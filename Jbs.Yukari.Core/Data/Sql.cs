using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Jbs.Yukari.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Jbs.Yukari.Core.Data
{
    public class Sql : ISql
    {
        private readonly IDatabase _database;
        const int commandTimeout = 600;

        public Sql(IDatabase database)
        {
            _database = database;
        }

        public async Task<IEnumerable<ListItem>> Search(SearchCriteria searchCriteria)
        {
            var rowsSelect = @"SELECT 
    basicinfo_id AS Yid,
    identity_no AS Id,
    type_id AS Type,
    name AS Name,
    status AS Status,
    phase_flag AS Phase,
    update_date AS WhenChanged,
    STUFF((
        SELECT
            ',' + object_type_id
        FROM Edit_ObjectInfo
        WHERE
            basicinfo_id = Edit_BasicInfo.basicinfo_id
        ORDER BY object_type_id
        FOR XML PATH('')
    ), 1, 1, '') AS Objects
FROM Edit_BasicInfo";
            var parameters = new DynamicParameters();
            var where = new List<string>();
            if (searchCriteria != null)
            {
                // ツリービューのノードを選択された場合
                if (!string.IsNullOrWhiteSpace(searchCriteria.SelectedNode))
                {
                    where.Add($@"basicinfo_id = @{nameof(searchCriteria.SelectedNode)} OR
basicinfo_id IN (
    SELECT
        basicinfo_id
    FROM Edit_BasicInfo_Membership
    WHERE
        parent_basicinfo_id = @{nameof(searchCriteria.SelectedNode)}
)");
                    parameters.Add(nameof(searchCriteria.SelectedNode), searchCriteria.SelectedNode);
                }
                // 条件検索された場合
                else
                {
                    if (!string.IsNullOrWhiteSpace(searchCriteria.Id))
                    {
                        where.Add($"identity_no LIKE @{nameof(searchCriteria.Id)}");
                        parameters.Add(nameof(searchCriteria.Id), $"{searchCriteria.Id.Replace("_", "[_]")}%");
                    }
                    if (!string.IsNullOrWhiteSpace(searchCriteria.Type))
                    {
                        where.Add($"type_id = @{nameof(searchCriteria.Type)}");
                        parameters.Add(nameof(searchCriteria.Type), searchCriteria.Type);
                    }
                    if (!string.IsNullOrWhiteSpace(searchCriteria.Name))
                    {
                        where.Add($"name LIKE @{nameof(searchCriteria.Name)}");
                        parameters.Add(nameof(searchCriteria.Name), $"%{searchCriteria.Name.Replace("_", "[_]")}%");
                    }
                }
            }
            var rowsSql = $"{rowsSelect}{(where.Any() ? " WHERE " + string.Join(" AND ", where) : null)} ORDER BY sort_no, identity_no";
            var records = _database.Connection.QueryAsync<ListItem>(rowsSql, parameters, null, commandTimeout);
            return await records;
        }

        public async Task<T> Get<T>(string yid)
        {
            var sql = @"SELECT
    basicinfo_id AS Yid,
    identity_no AS Id,
    type_id AS Type,
    name AS Name,
    phase_flag AS Phase,
    basicinfo_data AS Properties
FROM Edit_BasicInfo WHERE basicinfo_id = @yid";
            return await _database.Connection.QuerySingleAsync<T>(sql, new { yid }, null, commandTimeout);
        }

        public async Task<IEnumerable<Dictionary<string, Role>>> GetRole(string yid)
        {
            var sql = @"SELECT
    m.sort_no AS [Key],
    m.parent_basicinfo_id AS Yid,
    b.name AS Name,
    b.type_id AS Type
FROM Edit_BasicInfo_Membership m
INNER JOIN Edit_BasicInfo b ON m.parent_basicinfo_id = b.basicinfo_id
WHERE
    m.basicinfo_id = @yid AND
    b.type_id IN ('organization', 'title')
ORDER BY
    m.sort_no
";
            var grp = (await _database.Connection.QueryAsync(sql, new { yid }, null, commandTimeout))
                .GroupBy(x => x.Key)
                .Select(y => y.ToDictionary(z => (string)z.Type, a => new Role { Yid = a.Yid, Name = a.Name }));
            return grp;
        }

        public async Task<IEnumerable<T>> GetObjects<T>(string yid, string type)
        {
            var sql = @"SELECT
    O.basicinfo_id AS Yid,
    O.object_type_id AS Type,
    M.object_type_name AS TypeName,
    O.logon_name AS SamAccountName,
    O.display_name AS DisplayName,
    O.objectinfo_data AS Properties
FROM Edit_ObjectInfo AS O
INNER JOIN Mst_ObjectType M ON O.object_type_id = M.object_type_id
WHERE
    O.basicinfo_id = @yid AND
    M.object_class = @type
ORDER BY
    O.object_type_id";
            return await _database.Connection.QueryAsync<T>(sql, new { yid, type }, null, commandTimeout);
        }

        public async Task<string> GetTree(string type)
        {
            var sql = @"WITH organizations (yid, id, name, parentYid, sort) AS (
SELECT
	B.basicinfo_id,
    B.identity_no,
	B.name,
	BP.basicinfo_id,
	B.sort_no
FROM Edit_BasicInfo B
LEFT OUTER JOIN	Edit_BasicInfo_Membership BM ON B.basicinfo_id = BM.basicinfo_id
LEFT OUTER JOIN Edit_BasicInfo BP ON BM.parent_basicinfo_id = BP.basicinfo_id
WHERE
	B.type_id = @type AND
    B.status <> 2
), 
CTE(yid, level, id, text, parentYid, sort) 
AS (
    SELECT 
        yid,
		1,
        id,
        cast(name as nvarchar(max)),
		parentYid,
		sort
    FROM organizations
    WHERE parentYid is null 
    UNION ALL
    SELECT 
        OC.yid,
		level + 1,
        OC.id,
        --cast(CTE.text + '/' + OC.name as nvarchar(max)),
        cast(OC.name as nvarchar(max)),
		OC.parentYid,
		OC.sort
    FROM organizations OC
    INNER JOIN CTE ON OC.parentYid = CTE.yid
)
SELECT 
    *
FROM
    CTE
ORDER BY
    level,
	sort,
    id,
    text";
            TreeNode root = new TreeNode { Yid = Guid.NewGuid(), Text = type, ParentYid = Guid.Empty };
            var list = await _database.Connection.QueryAsync<TreeNode>(sql, new { type }, null, commandTimeout);
            foreach (TreeNode node in list)
            {
                if (node.ParentYid != Guid.Empty)
                {
                    list.Single(_ => _.Yid == node.ParentYid).Nodes.Add(node);
                }
                else
                {
                    node.ParentYid = root.Yid;
                    root.Nodes.Add(node);
                }
            }
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return $"[{JsonConvert.SerializeObject(root, settings)}]";
        }

        public async void Save(BasicInfo info)
        {
            using var transaction = _database.GetCurrentTransaction();
            try
            {
                var sql = $@"UPDATE Edit_BasicInfo
SET
    identity_no = @{nameof(info.Id)},
    name = @{nameof(info.Name)},
    basicinfo_data = @{nameof(info.Properties)},
    phase_flag = 2,
    update_date = GETDATE(),
    commit_date = GETDATE()
WHERE
    basicinfo_id = @{nameof(info.Yid)}
";
                var parameters = new DynamicParameters();
                parameters.Add($"@{nameof(info.Id)}", info.Id);
                parameters.Add($"@{nameof(info.Name)}", info.Name);
                parameters.Add($"@{nameof(info.Properties)}", info.Properties);
                parameters.Add($"@{nameof(info.Yid)}", info.Yid);
                await _database.Connection.ExecuteAsync(sql, parameters, transaction);

                sql = $@"UPDATE Edit_ObjectInfo
";
                var paramUser = new DynamicParameters();
                if (info.Users != null)
                {
                    foreach (var obj in info.Users)
                    {
                    }
                }
                var paramGroup = new DynamicParameters();
                if (info.Groups != null)
                {
                    foreach (var obj in info.Groups)
                    {
                    }
                }
                await _database.Connection.ExecuteAsync(sql, new { paramUser, paramGroup }, transaction);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async void Publish(Guid yid)
        {
            var sql = $@"UPDATE Edit_BasicInfo SET
phase_flag = 0,
reflect_date = GETDATE()
WHERE
    basicinfo_id = @yid";
            var parameters = new DynamicParameters();
            parameters.Add($"@yid", yid);
            await _database.Connection.ExecuteAsync(sql, parameters);
        }
    }
}
