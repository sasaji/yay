CREATE TABLE Edit_ObjectInfo_GroupMembership(
    group_object_id         uniqueidentifier    NOT NULL,
    member_object_id        uniqueidentifier    NOT NULL,
    add_date                datetime            NOT NULL,
    delete_expected_date    datetime            NULL,
    CONSTRAINT PK91_1 PRIMARY KEY CLUSTERED (group_object_id, member_object_id)
)
