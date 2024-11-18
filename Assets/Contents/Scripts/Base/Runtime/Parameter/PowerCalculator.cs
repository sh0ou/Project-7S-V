using System;
using System.Text.RegularExpressions;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace sh0uRoom.PJ7S
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class PowerCalculator : UdonSharpBehaviour
    {
        private void TestCalculatePower()
        {
            // 仮のパラメータを設定
            currentP.SetParameter(PlayerParameterType.Atk, 100);
            targetP.SetParameter(PlayerParameterType.Def, 50);

            var power = CalculatePower();
            Debug.Log($"{DEBUG_HEAD}Power: {power}");
        }

        /// <summary>
        /// パワーを計算
        /// </summary>
        /// <returns>パワー値</returns>
        public int CalculatePower()
        {
            if (currentP == null || targetP == null)
            {
                Debug.LogError($"{DEBUG_HEAD}ParameterableCharacter is null.");
                return 0;
            }

            // 変数を実際の値に置換
            var parsedFormula = formula
                // 自身
                .Replace("a.lv", currentP.GetParameter(PlayerParameterType.Lv).ToString())
                .Replace("a.hp", currentP.GetParameter(PlayerParameterType.Hp).ToString())
                .Replace("a.mhp", currentP.GetParameter(PlayerParameterType.MaxHp).ToString())
                .Replace("a.mp", currentP.GetParameter(PlayerParameterType.Mp).ToString())
                .Replace("a.mmp", currentP.GetParameter(PlayerParameterType.MaxMp).ToString())
                .Replace("a.sp", currentP.GetParameter(PlayerParameterType.Tp).ToString())
                .Replace("a.mtp", currentP.GetParameter(PlayerParameterType.MaxTp).ToString())
                .Replace("a.atk", currentP.GetParameter(PlayerParameterType.Atk).ToString())
                .Replace("a.def", currentP.GetParameter(PlayerParameterType.Def).ToString())
                .Replace("a.mat", currentP.GetParameter(PlayerParameterType.Mat).ToString())
                .Replace("a.mdf", currentP.GetParameter(PlayerParameterType.Mdf).ToString())
                .Replace("a.spd", currentP.GetParameter(PlayerParameterType.Spd).ToString())
                .Replace("a.luk", currentP.GetParameter(PlayerParameterType.Luk).ToString())
                // 対象
                .Replace("b.lv", targetP.GetParameter(PlayerParameterType.Lv).ToString())
                .Replace("b.hp", targetP.GetParameter(PlayerParameterType.Hp).ToString())
                .Replace("b.mhp", targetP.GetParameter(PlayerParameterType.MaxHp).ToString())
                .Replace("b.mp", targetP.GetParameter(PlayerParameterType.Mp).ToString())
                .Replace("b.mmp", targetP.GetParameter(PlayerParameterType.MaxMp).ToString())
                .Replace("b.sp", targetP.GetParameter(PlayerParameterType.Tp).ToString())
                .Replace("b.mtp", targetP.GetParameter(PlayerParameterType.MaxTp).ToString())
                .Replace("b.atk", targetP.GetParameter(PlayerParameterType.Atk).ToString())
                .Replace("b.def", targetP.GetParameter(PlayerParameterType.Def).ToString())
                .Replace("b.mat", targetP.GetParameter(PlayerParameterType.Mat).ToString())
                .Replace("b.mdf", targetP.GetParameter(PlayerParameterType.Mdf).ToString())
                .Replace("b.spd", targetP.GetParameter(PlayerParameterType.Spd).ToString())
                .Replace("b.luk", targetP.GetParameter(PlayerParameterType.Luk).ToString());

            tokens = Regex.Split(parsedFormula, @"([+\-*/()])");

            return !IsSafeTokens() ? currentP.GetParameter(PlayerParameterType.Atk) : EvaluateExpression();
        }

        private bool IsSafeTokens()
        {
            if (tokens.Length == 0) return false;

            //空トークンの削除
            var rawTokens = tokens;
            var nonEmptyCount = 0;
            foreach (var token in rawTokens)
            {
                if (string.IsNullOrEmpty(token)) continue;
                nonEmptyCount++;
            }

            tokens = new string[nonEmptyCount];
            var index = 0;

            foreach (var token in rawTokens)
            {
                if (string.IsNullOrEmpty(token)) continue;
                tokens[index++] = token;
            }

            // Debug.Log($"{DEBUG_HEAD}TokenCount: {tokens.Length}");

            return true;
        }

        /// <summary>
        /// 式を解析し計算（四則演算と括弧のみ対応）
        /// </summary>
        /// <param name="parsedFormula"></param>
        /// <returns></returns>
        private int EvaluateExpression()
        {
            var values = new int[64];
            var operators = new char[64];
            var valueIndex = -1;
            var operatorIndex = -1;

            foreach (var token in tokens)
            {
                // Debug.Log($"{DEBUG_HEAD}Token: {token}");
                if (int.TryParse(token, out var number))
                {
                    values[++valueIndex] = number;
                }
                else if (token == "(")
                {
                    operators[++operatorIndex] = '(';
                }
                else if (token == ")")
                {
                    // 開き括弧に到達するまで演算を実行
                    while (operatorIndex >= 0 && operators[operatorIndex] != '(')
                    {
                        ApplyOperator(ref values, ref operators, ref valueIndex, ref operatorIndex);
                    }
                    if (operatorIndex >= 0) operatorIndex--; // 開き括弧をスタックから取り除く
                }
                else
                {
                    var op = token[0];
                    // オペレータスタック上の演算子の優先順位を確認し、適用する
                    while (operatorIndex >= 0 && operators[operatorIndex] != '(' && Precedence(operators[operatorIndex]) >= Precedence(op))
                    {
                        ApplyOperator(ref values, ref operators, ref valueIndex, ref operatorIndex);
                    }
                    // 演算子をスタックに積む
                    operators[++operatorIndex] = op;
                }
            }

            while (operatorIndex >= 0)
            {
                ApplyOperator(ref values, ref operators, ref valueIndex, ref operatorIndex);
            }

            return values[valueIndex];
        }

        /// <summary>
        /// 演算子の優先順位を返す
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        private static int Precedence(char op) => (op == '+' || op == '-') ? 1 : 2;

        /// <summary>
        /// 演算子を適用
        /// </summary>
        /// <param name="values"></param>
        /// <param name="operators"></param>
        /// <param name="valueIndex"></param>
        /// <param name="operatorIndex"></param>
        private static void ApplyOperator(ref int[] values, ref char[] operators, ref int valueIndex, ref int operatorIndex)
        {
            var right = values[valueIndex--];
            var left = values[valueIndex--];
            var op = operators[operatorIndex--];

            switch (op)
            {
                case '+':
                    values[++valueIndex] = left + right;
                    break;
                case '-':
                    values[++valueIndex] = left - right;
                    break;
                case '*':
                    values[++valueIndex] = left * right;
                    break;
                case '/':
                    values[++valueIndex] = left / right;
                    break;
            }
        }

        /* 任意の計算式を設定
        vxaceのダメージ計算式を採用

        mhp	最大 HP
        mmp	最大 MP
        mtp 最大 TP
        hp	現在の HP
        mp	現在の MP
        tp	現在の TP
        atk	攻撃力
        def	防御力
        mat 魔法攻撃力
        mdf 魔法防御力
        spd	素早さ
        luk	運
        lv レベル

        a 使用者
        b 対象

        使用可能な符号 +, -, *, /, (, )

        *1 設定した計算式が無効もしくはエラー（ゼロ除算など）の場合はa.atkとなる
        *2 小数は繰り上げ
        */

        public string formula = "a.atk*4-b.def*2";
        public ParameterableCharacter currentP;
        public ParameterableCharacter targetP;

        [SerializeField] private string[] tokens = Array.Empty<string>();

        private const string DEBUG_HEAD = "[<color=yellow>PowerCalculator</color>]";
    }
}