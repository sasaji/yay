﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NKakasi;

namespace Jbs.Yukari.Core.Services.Romanization
{
    class KakasiWrapper
    {
        public List<string> Do(string source, bool isKunrei)
        {
            // kakasi に渡すパラメーターを設定（詳細は kakasi の man ページを参照）。
            List<string> argv = new List<string> { "-Ja", "-Ha", "-Ka", "-p" };

            // 訓令式が指定されていたら -rk を付与する。
            if (isKunrei)
                argv.Add("-rkunrei");

            // kakasi による変換を実行。
            // "簡易" は "kan'i" のように変換されたりするので記号を除外する。
            var kakasi = new Kakasi();
            kakasi.SetArguments(argv.ToArray());
            string kakasiDone = Regex.Replace(kakasi.DoString(source), @"['\^]", string.Empty);

            // ヘボン式の撥音で b、m、p の前の n が m にならないので変換する。
            if (!isKunrei)
                kakasiDone = kakasiDone.Replace("nb", "mb").Replace("nm", "mm").Replace("np", "mp");

            // ヘボン式の場合 cch は tch に変換する。
            if (!isKunrei)
                kakasiDone = kakasiDone.Replace("cch", "tch");

            // 変換候補が複数ある場合は {a|b|c} のように変換される。
            // また文節変換された場合は {a|b|c}{d|e} のように変換される。
            // 正規表現を用いて {} で囲まれた部分の文字列を取り出す。
            MatchCollection matches = Regex.Matches(kakasiDone, @"{.+?}");

            // 正規表現にマッチしなかった場合は変換候補が単数なので結果をそのまま返す。
            if (matches.Count == 0)
                return new List<string>() { kakasiDone };

            // kakasi の変換結果を List<List<string>> の形にする。
            List<List<string>> kakasiList = new List<List<string>>();
            matches.Cast<Match>().Select(i => i.Value.TrimStart('{').TrimEnd('}'))
                .ToList().ForEach(i => kakasiList.Add(i.Split('|').ToList()));

            // 文節変換された各要素の直積を求めてそれを結果とする。
            // たとえば、{a|b|c}{d|e} が得られた場合は、{ad, ae, bd, be, cd, ce} を返す。
            List<string> result = new List<string>();
            kakasiList.Aggregate(
                Enumerable.Repeat(Enumerable.Empty<string>(), 1),
                (a, b) =>
                    from x in a
                    from y in b
                    select x.Concat(Enumerable.Repeat(y, 1)))
                .ToList().ForEach(i => result.Add(string.Join(string.Empty, i)));
            return result;
        }
    }
}
