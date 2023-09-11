using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace CalamityWeaponRemake.Common.AuxiliaryMeans
{
    /// <summary>
    /// 提供一些算法工具与常量
    /// </summary>
    public static class HcMath
    {
        public static Random HcRandom = new Random();

        /// <summary>
        /// 角度插值
        /// </summary>
        /// <param name="startAngle">起始角度</param>
        /// <param name="targetAngle">目标角度</param>
        /// <param name="amount">插值算子</param>
        /// <returns></returns>
        public static float LerpAngle(float startAngle, float targetAngle, float amount)
        {
            float difference = MathHelper.WrapAngle(targetAngle - startAngle);
            return MathHelper.WrapAngle(startAngle + difference * amount);
        }

        /// <summary>
        /// 用于逐渐将一个角度从当前角度（rot）调整到目标角度（toRot），以给定的旋转速度（rotSpeed）
        /// 角度值被限制在 -π 到 π 的范围内，以确保旋转在合适的角度范围内进行
        /// </summary>
        /// <param name="rot">当前角度</param>
        /// <param name="toRot">目标角度，要旋转到的角度</param>
        /// <param name="rotSpeed">旋转速度，控制旋转的幅度</param>
        /// <returns>调整后的角度值，逐渐接近目标角度</returns>
        public static float GraduallyToRot(float rot, float toRot, float rotSpeed)
        {
            // 将角度限制在 -π 到 π 的范围内
            rot = MathHelper.WrapAngle(rot);

            // 计算差异角度
            float diff = MathHelper.WrapAngle(toRot - rot);

            // 选择修改幅度小的方向进行旋转
            if (Math.Abs(diff) < MathHelper.Pi)
            {
                rot += diff * rotSpeed;
            }
            else
            {
                rot -= MathHelper.WrapAngle(-diff) * rotSpeed;
            }

            return rot;
        }

        /// <summary>
        /// 比较两个角度之间的差异，将结果限制在 -π 到 π 的范围内
        /// </summary>
        /// <param name="baseAngle">基准角度（参考角度）</param>
        /// <param name="targetAngle">目标角度（待比较角度）</param>
        /// <returns>从基准角度到目标角度的差异，范围在 -π 到 π 之间</returns>
        public static float CompareAngle(float baseAngle, float targetAngle)
        {
            // 计算两个角度之间的差异并将结果限制在 -π 到 π 的范围内
            return (baseAngle - targetAngle + (float)Math.PI * 3) % MathHelper.TwoPi - (float)Math.PI;
        }

        /// <summary>
        /// 将给定的角度值转换为以 π 为中心的标准化角度
        /// </summary>
        /// <param name="angleIn">输入的角度值</param>
        /// <returns>标准化的角度，以 π 为中心，范围在 -π 到 π 之间</returns>
        public static float ConvertAngle(float angleIn)
        {
            // 将输入角度与零角度比较，以获得标准化的角度
            return CompareAngle(0, angleIn) + (float)Math.PI;
        }

        /// <summary>
        ///  HSV 到 RGB 的转换
        /// </summary>
        /// <param name="hue">色相</param>
        /// <param name="saturation">饱和度</param>
        /// <param name="value">亮度</param>
        /// <returns></returns>
        public static Color ColorFromHSV(float hue, float saturation, float value)
        {
            int hi = (int)Math.Floor(hue / 60) % 6;
            float f = hue / 60 - (float)Math.Floor(hue / 60);

            value = value * 255;
            int v = (int)value;
            int p = (int)(value * (1 - saturation));
            int q = (int)(value * (1 - f * saturation));
            int t = (int)(value * (1 - (1 - f) * saturation));

            if (hi == 0)
            {
                return new Color(v, t, p);
            }
            else if (hi == 1)
            {
                return new Color(q, v, p);
            }
            else if (hi == 2)
            {
                return new Color(p, v, t);
            }
            else if (hi == 3)
            {
                return new Color(p, q, v);
            }
            else if (hi == 4)
            {
                return new Color(t, p, v);
            }
            else
            {
                return new Color(v, p, q);
            }
        }

        /// <summary>
        /// 色彩混合
        /// </summary>
        public static Color RecombinationColor(params (Color color, float weight)[] colorWeightPairs)
        {
            Vector4 result = Vector4.Zero;

            for (int i = 0; i < colorWeightPairs.Length; i++)
            {
                result += colorWeightPairs[i].color.ToVector4() * colorWeightPairs[i].weight;
            }

            return new Color(result);
        }

        public static Vector2 To(this Vector2 vr1, Vector2 vr2) => vr2 - vr1;

        /// <summary>
        /// 获取一个随机方向的向量
        /// </summary>
        /// <param name="startAngle">开始角度,请输入角度单位的值</param>
        /// <param name="targetAngle">目标角度,请输入角度单位的值</param>
        /// <param name="ModeLength">返回的向量的长度</param>
        /// <returns></returns>
        public static Vector2 GetRandomVevtor(float startAngle, float targetAngle, float ModeLength)
        {
            float angularSeparation = targetAngle - startAngle;
            double randomPosx = (angularSeparation * HcRandom.NextDouble() + startAngle) * (MathHelper.Pi / 180);
            return new Vector2((float)Math.Cos(randomPosx), (float)Math.Sin(randomPosx)) * ModeLength;
        }

        /// <summary>
        /// 获取一个垂直于该向量的单位向量
        /// </summary>
        public static Vector2 GetNormalVector(this Vector2 vr)
        {
            Vector2 nVr = new Vector2(vr.Y, -vr.X);
            return Vector2.Normalize(nVr);
        }

        /// <summary>
        /// 简单安全的获取一个单位向量，如果出现非法情况则会返回 <see cref="Vector2.Zero"/>
        /// </summary>
        public static Vector2 UnitVector(this Vector2 vr)
        {
            Vector2 newVr = vr.SafeNormalize(Vector2.Zero);
            return newVr;
        }

        /// <summary>
        /// 计算两个向量的点积
        /// </summary>
        public static float DotProduct(this Vector2 vr1, Vector2 vr2)
        {
            return vr1.X * vr2.X + vr1.Y * vr2.Y;
        }

        /// <summary>
        /// 计算两个向量的叉积
        /// </summary>
        public static float CrossProduct(this Vector2 vr1, Vector2 vr2)
        {
            return vr1.X * vr2.Y - vr1.Y * vr2.X;
        }

        /// <summary>
        /// 获取向量与另一个向量的夹角
        /// </summary>
        /// <returns>返回劣弧角，即取值范围为 0 到 π 弧度之间</returns>
        public static float VetorialAngle(this Vector2 vr1, Vector2 vr2)
        {
            return (float)Math.Acos(vr1.DotProduct(vr2) / (vr1.Length() * vr2.Length()));
        }

        /// <summary>
        /// 计算从中心点到目标点的角度（以弧度为单位）
        /// </summary>
        /// <param name="destination">目标点</param>
        /// <param name="center">中心点</param>
        /// <returns>从中心点到目标点的角度（以弧度为单位）</returns>
        public static float AngleTo(Vector2 destination, Vector2 center)
        {
            return (float)Math.Atan2(destination.Y - center.Y, destination.X - center.X);
        }

        /// <summary>
        /// 一个随机布尔值获取方法
        /// </summary>
        /// <param name="ProbabilityDenominator">概率分母</param>
        /// <param name="ProbabilityExpectation">期望倾向</param>
        /// <param name="DesiredObject">期望对象</param>
        /// <returns></returns>
        public static bool RandomBooleanValue(int ProbabilityDenominator, int ProbabilityExpectation, bool DesiredObject)
        {
            int randomInt = (int)HcRandom.NextInt64(0, ProbabilityDenominator);

            if (DesiredObject && randomInt == ProbabilityExpectation)
            {
                return true;
            }
            else if (randomInt == ProbabilityExpectation)
            {
                return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据向量的Y值来进行比较
        /// </summary>
        public class VeYSort : IComparer<Vector2>
        {
            public int Compare(Vector2 v1, Vector2 v2)
            {
                // 比较两个向量的Y值，根据Y值大小进行排序
                if (v1.Y < v2.Y)
                    return -1;
                else if (v1.Y > v2.Y)
                    return 1;
                else
                    return 0;
            }
        }

        /// <summary>
        /// 获取一组离散点
        /// </summary>
        /// <param name="numPoints">点数量，这将决定数组的索引值</param>
        /// <param name="Length">随机长度</param>
        /// <param name="Deviation">随机宽度</param>
        /// <param name="startPosition">开始位置</param>
        /// <param name="destinationPosition">结束位置，如果不输入则默认让点的分布末端离散</param>
        /// <returns>返回一串向量值类型的点数组</returns>
        public static Vector2[] GetRandomPos(int numPoints, float Length, float Deviation, Vector2 startPosition, Vector2 destinationPosition = default)
        {
            Vector2[] PosNillm = new Vector2[numPoints];
            Vector2 Pos = startPosition;
            float intervalY = Length / numPoints;
            float NewInterval = 0;
            if (destinationPosition == default)
            {
                for (int i = 0; i < numPoints; i++)
                {
                    intervalY += (float)HcRandom.NextDouble() * intervalY * 0.15f * HcRandom.Next(-1, 2);
                    NewInterval += intervalY;
                    float intervalX = HcRandom.Next(-1, 2) * NewInterval;
                    Vector2 RandomVr = new Vector2(intervalX, intervalY);
                    Pos += RandomVr;
                    PosNillm[i] = Pos;
                }
                return PosNillm;
            }
            else
            {
                for (int i = 0; i < numPoints / 2; i++)
                {
                    intervalY += (float)HcRandom.NextDouble() * intervalY * 0.15f * HcRandom.Next(-1, 2);
                    NewInterval += intervalY;
                    float intervalX = HcRandom.Next(-1, 2) * NewInterval;
                    Vector2 RandomVr = new Vector2(intervalX, intervalY);
                    Pos += RandomVr;
                    PosNillm[i] = Pos;
                }
                for (int i = numPoints / 2; i < numPoints; i++)
                {
                    intervalY += (float)HcRandom.NextDouble() * intervalY * 0.15f * HcRandom.Next(-1, 2);
                    NewInterval -= intervalY;
                    float intervalX = HcRandom.Next(-1, 2) * NewInterval;
                    Vector2 RandomVr = new Vector2(intervalX, intervalY);
                    Pos += RandomVr;
                    PosNillm[i] = Pos;
                }
                return PosNillm;
            }
        }

        public static void GenerateCurve(ref List<Vector2> points, Vector2 startPoint, Vector2 endPoint, int numPoints, Vector2 controlPoint, float gravityFactor)
        {
            points = new List<Vector2>();

            if (controlPoint == Vector2.Zero) controlPoint = (startPoint + endPoint) / 2f;

            float curveLength = Vector2.Distance(startPoint, endPoint);    // 曲线长度

            for (int i = 0; i < numPoints; i++)
            {
                float t = (float)i / (numPoints - 1);

                // 根据贝塞尔曲线公式计算点的位置
                float u = 1f - t;
                float tt = t * t;
                float uu = u * u;
                float uuu = uu * u;
                float ttt = tt * t;

                // 根据双曲函数调整控制点的 Y 坐标
                float adjustedControlPointY = controlPoint.Y + curveLength * gravityFactor * (float)Math.Tanh(t * Math.PI);

                Vector2 adjustedControlPoint = new Vector2(controlPoint.X, adjustedControlPointY);

                Vector2 point = uuu * startPoint + 3f * uu * t * adjustedControlPoint + 3f * u * tt * adjustedControlPoint + ttt * endPoint;

                points.Add(point);
            }
        }

        /// <summary>
        /// 生成一条二次贝塞尔曲线上的点集合，该曲线由起点、终点、控制点和重力因子定义
        /// </summary>
        /// <param name="points">生成的点集合将被存储在此列表中</param>
        /// <param name="startPoint">曲线的起始点</param>
        /// <param name="endPoint">曲线的终点</param>
        /// <param name="numPoints">生成的点的数量，用于定义曲线的平滑度</param>
        /// <param name="controlPoint">控制点，用于调整曲线的形状如果为 Vector2.Zero，则使用起点和终点的中点</param>
        /// <param name="gravityFactor">重力因子，用于调整控制点的 Y 坐标，影响曲线的形状</param>
        public static void GenerateCurve2(ref List<Vector2> points, Vector2 startPoint, Vector2 endPoint, int numPoints, Vector2 controlPoint, float gravityFactor)
        {
            points = new List<Vector2>();

            if (controlPoint == Vector2.Zero) controlPoint = (startPoint + endPoint) / 2f;

            float curveLength = Vector2.Distance(startPoint, endPoint);    // 曲线长度

            for (int i = 0; i < numPoints; i++)
            {
                float t = (float)i / (numPoints - 1);

                // 根据贝塞尔曲线公式计算点的位置
                float u = 1f - t;
                float tt = t * t;
                float uu = u * u;
                float uuu = uu * u;
                float ttt = tt * t;

                // 根据重力因子调整控制点的 Y 坐标
                Vector2 adjustedControlPoint = new Vector2(controlPoint.X, controlPoint.Y + curveLength * gravityFactor);

                Vector2 point = uuu * startPoint + 3f * uu * t * adjustedControlPoint + 3f * u * tt * adjustedControlPoint + ttt * endPoint;

                points.Add(point);

            }
        }

        /// <summary>
        /// 获取近似的样条长度，通过分割样条曲线为多个小段并计算它们的长度来近似总长度
        /// </summary>
        /// <param name="steps">分割样条曲线的段数，段数越多，计算越精确但性能开销越大</param>
        /// <param name="start">样条曲线的起点</param>
        /// <param name="startTan">起点处的切线向量，影响曲线的方向</param>
        /// <param name="end">样条曲线的终点</param>
        /// <param name="endTan">终点处的切线向量，影响曲线的方向</param>
        /// <returns>近似的样条长度</returns>
        public static float ApproximateSplineLength(int steps, Vector2 start, Vector2 startTan, Vector2 end, Vector2 endTan)
        {
            float total = 0;
            Vector2 prevPoint = start;

            for (int k = 0; k < steps; k++)
            {
                var testPoint = Vector2.Hermite(start, startTan, end, endTan, k / (float)steps);
                total += Vector2.Distance(prevPoint, testPoint);

                prevPoint = testPoint;
            }

            return total;
        }

        /// <summary>
        /// 计算贝塞尔样条上的点位置，根据进度和给定的控制点信息
        /// </summary>
        /// <param name="progress">进度，通常在 0 到 1 之间，表示在样条上的位置</param>
        /// <param name="dist1">起点到中间控制点之间的距离</param>
        /// <param name="dist2">中间控制点到终点之间的距离</param>
        /// <param name="startPoint">起始点的坐标</param>
        /// <param name="targetPoint">目标点的坐标</param>
        /// <param name="Midpoint">中间控制点的坐标</param>
        /// <returns>贝塞尔样条上的点位置</returns>
        public static Vector2 PointOnSpline(float progress, float dist1, float dist2, Vector2 startPoint, Vector2 targetPoint, Vector2 Midpoint)
        {
            float factor = dist1 / (dist1 + dist2);

            if (progress < factor)
                return Vector2.Hermite(startPoint, Midpoint - startPoint, Midpoint, targetPoint - startPoint, progress * (1 / factor));

            if (progress >= factor)
                return Vector2.Hermite(Midpoint, targetPoint - startPoint, targetPoint, targetPoint - Midpoint, (progress - factor) * (1 / (1 - factor)));

            return Vector2.Zero;
        }

        /// <summary>
        /// 用于截取浮点数
        /// </summary>
        /// <param name="number">截取对象,需要避免输入含0位数的浮点值,比如1.00200,这可能因为精度问题导致函数报错,需要输入整数为单位数的浮点数,如1.234,不能输入12.34,或者465,这会引发预想之外的结果</param>
        /// <param name="highestPosition">结束的位数,位数是从第0位开始算,比如2.718,第0位数是2,第3位数是8</param>
        /// <param name="lowestPosition">开始的位数,位数是从第0位开始算,比如2.718,第0位数是2,第3位数是8</param>
        /// <returns></returns>
        public static int GetDigitsInRange(float number, int highestPosition, int lowestPosition)
        {
            if (highestPosition < lowestPosition || lowestPosition < 0)
            {
                return -1;
            }

            // 将浮点数转换为字符串并去除小数点
            string numberString = number.ToString().Replace(".", "");

            // 创建一个数组来存储数字
            int[] digits = new int[numberString.Length];

            // 将字符串中的每个字符解析为整数并存储在数组中
            for (int i = 0; i < numberString.Length; i++)
            {
                digits[i] = int.Parse(numberString[i].ToString());
            }

            // 从数组中获取指定范围的数字
            int[] result = new int[highestPosition - lowestPosition + 1];
            Array.Copy(digits, lowestPosition, result, 0, highestPosition - lowestPosition + 1);
            int extractedNumber = 0;
            for (int i = 0; i < result.Length; i++)
            {
                extractedNumber = extractedNumber * 10 + result[i];
            }

            return extractedNumber;
        }

        /// <summary>
        /// 转化浮点数为整数集合
        /// </summary>
        /// <param name="number">目标浮点数</param>
        /// <returns></returns>
        public static int[] GetDigitsArray(float number)
        {
            // 将浮点数转换为字符串
            string numberStr = number.ToString("F15");

            // 去除小数点，保留整数部分
            string integerPart = numberStr.Split('.')[0];

            // 将整数部分转换为字符数组
            char[] charArray = integerPart.ToCharArray();

            // 将字符数组转换为整数数组
            int[] digitsArray = new int[charArray.Length];
            for (int i = 0; i < charArray.Length; i++)
            {
                digitsArray[i] = charArray[i] - '0';
            }

            return digitsArray;
        }

        /// <summary>
        /// 使用贝塞尔曲线算法生成离散的点集，常用于绘制不规则路径线条，如闪电
        /// </summary>
        public class LightningGenerator
        {
            public static Vector2[] GenerateLightning(Vector2 startPoint, Vector2 endPoint, int numPoints, float deviation)
            {
                Vector2[] controlPoints = GetControlPoints(startPoint, endPoint, numPoints, deviation);
                Vector2[] lightningPath = GenerateBezierCurve(controlPoints, numPoints);

                return lightningPath;
            }

            private static Vector2[] GetControlPoints(Vector2 startPoint, Vector2 endPoint, int numPoints, float deviation)
            {
                Vector2[] controlPoints = new Vector2[numPoints + 2];
                controlPoints[0] = startPoint;
                controlPoints[numPoints + 1] = endPoint;

                float length = Vector2.Distance(startPoint, endPoint);
                float segmentLength = length / (numPoints + 1);

                for (int i = 1; i <= numPoints; i++)
                {
                    float t = i / (float)(numPoints + 1);
                    float offset = deviation * (1 - 2 * t);
                    Vector2 direction = Vector2.Normalize(endPoint - startPoint);
                    Vector2 perpendicular = new Vector2(-direction.Y, direction.X);
                    Vector2 controlPoint = startPoint + t * (endPoint - startPoint) + offset * perpendicular;

                    controlPoints[i] = controlPoint;
                }

                return controlPoints;
            }

            private static Vector2[] GenerateBezierCurve(Vector2[] controlPoints, int numPoints)
            {
                Vector2[] curvePoints = new Vector2[numPoints];
                for (int i = 0; i < numPoints; i++)
                {
                    float t = (i + 1) / (float)(numPoints + 1);
                    curvePoints[i] = BezierInterpolation(controlPoints, t);
                }

                return curvePoints;
            }

            private static Vector2 BezierInterpolation(Vector2[] points, float t)
            {
                int n = points.Length - 1;
                Vector2 result = Vector2.Zero;

                for (int i = 0; i <= n; i++)
                {
                    float binomialCoefficient = BinomialCoefficient(n, i);
                    float powerT = (float)Math.Pow(t, i);
                    float powerOneMinusT = (float)Math.Pow(1 - t, n - i);
                    result += binomialCoefficient * powerT * powerOneMinusT * points[i];
                }

                return result;
            }

            private static int BinomialCoefficient(int n, int k)
            {
                if (k == 0 || k == n)
                {
                    return 1;
                }

                int[] coefficients = new int[n + 1];
                coefficients[0] = 1;

                for (int i = 1; i <= n; i++)
                {
                    coefficients[i] = 1;
                    for (int j = i - 1; j > 0; j--)
                    {
                        coefficients[j] += coefficients[j - 1];
                    }
                }

                return coefficients[k];
            }
        }

        /// <summary>
        /// 将游戏中的极角值转化为顺时针的正角值，处理角度对象
        /// </summary>
        public static float PolarToAngle_D(float polar)
        {
            if (polar < 0)
            {
                return 360 + polar;
            }
            else
            {
                return polar;
            }
        }

        /// <summary>
        /// 将游戏中的极角值转化为顺时针的正角值，处理弧度对象
        /// </summary>
        public static float PolarToAngle_R(float polar)
        {
            polar = MathHelper.ToRadians(polar);
            if (polar < 0) return MathHelper.TwoPi + polar;
            else return polar;
        }

        /// <summary>
        /// 检测索引的合法性
        /// </summary>
        /// <returns>合法将返回 <see cref="true"/></returns>
        public static bool ValidateIndex(Array array, int index)
        {
            if (index < 0 || index > array.Length - 1) return false;
            else return true;
        }


        /// <summary>
        /// 检测索引的合法性
        /// </summary>
        /// <returns>合法将返回 <see cref="true"/></returns>
        public static bool ValidateIndex(List<int> ts, int index)
        {

            if (index < 0 || index > ts.Count - 1) return false;
            else return true;
        }

        public static bool WithinBounds(this int index, int cap)
        {
            if (index >= 0)
            {
                return index < cap;
            }
            return false;
        }

        /// <summary>
        /// 会自动替补-1元素
        /// </summary>
        /// <param name="list">目标集合</param>
        /// <param name="valueToAdd">替换为什么值</param>
        /// <param name="valueToReplace">替换的目标对象的值，不填则默认为-1</param>
        public static void AddOrReplace(this List<int> list, int valueToAdd, int valueToReplace = -1)
        {
            int index = list.IndexOf(valueToReplace);
            if (index >= 0)
            {
                list[index] = valueToAdd;
            }
            else
            {
                list.Add(valueToAdd);
            }
        }

        /// <summary>
        /// 返回一个集合的干净数量，排除数默认为-1，该扩展方法不会影响原集合
        /// </summary>
        public static int GetIntListCount(this List<int> list, int valueToReplace = -1)
        {
            List<int> result = new List<int>(list);
            result.RemoveAll(item => item == -1);
            return result.Count;
        }

        /// <summary>
        /// 返回一个集合的筛选副本，排除数默认为-1，该扩展方法不会影响原集合
        /// </summary>
        public static List<int> GetIntList(this List<int> list, int valueToReplace = -1)
        {
            List<int> result = new List<int>(list);
            result.RemoveAll(item => item == -1);
            return result;
        }

        /// <summary>
        /// 去除目标集合中所有-1元素
        /// </summary>
        /// <param name="list"></param>
        public static void SweepLoadLists(ref List<int> list)
        {
            if (list.Count > 0 && list.All(num => num == -1))
            {
                list.RemoveAll(num => num == -1);
            }
        }

        /// <summary>
        /// 单独的重载集合方法
        /// </summary>
        public static void UnLoadList(ref List<int> Lists)
        {
            Lists = new List<int>();
        }

        /// <summary>
        /// 将数组克隆出一份List类型
        /// </summary>
        public static List<T> ToList<T>(this T[] array)
        {
            return new List<T>(array);
        }

        /// <summary>
        /// 对float集合进行平滑插值，precision不应该输入0值或者负值
        /// </summary>
        public static List<float> InterpolateFloatList(List<float> originalList, float precision)
        {
            if (precision <= 0) precision = 1;
            int precisionCounter = (int)(1f / precision);
            if (precisionCounter < 1) precisionCounter = 1;

            List<float> interpolatedList = new List<float>();

            for (int i = 0; i < originalList.Count - 1; i++)
            {
                interpolatedList.Add(originalList[i]);

                float currentValue = originalList[i];
                float nextValue = originalList[i + 1];
                float dis = nextValue - currentValue;
                float absDis = Math.Abs(dis);
                int numInterpolations = (int)absDis * precisionCounter;

                for (int j = 1; j <= numInterpolations; j++)
                {
                    float t = j / (float)(numInterpolations + 1);
                    float interpolatedValue = MathHelper.Lerp(currentValue, nextValue, t);
                    interpolatedList.Add(interpolatedValue);
                }
            }

            interpolatedList.Add(originalList[originalList.Count - 1]);

            return interpolatedList;
        }

        /// <summary>
        /// 对Vector2集合进行平滑插值，precision不应该输入0值或者负值
        /// </summary>
        public static List<Vector2> InterpolateVectorList(List<Vector2> originalList, float precision = 1)
        {
            if (precision <= 0) precision = 1;
            int precisionCounter = (int)(1f / precision);
            if (precisionCounter < 1) precisionCounter = 1;

            List<Vector2> interpolatedList = new List<Vector2>();

            for (int i = 0; i < originalList.Count - 1; i++)
            {
                interpolatedList.Add(originalList[i]);

                Vector2 currentValue = originalList[i];
                Vector2 nextValue = originalList[i + 1];
                float disX = Math.Abs(nextValue.X - currentValue.X);
                float disY = Math.Abs(nextValue.Y - currentValue.Y);
                float maxDis = Math.Max(disX, disY);
                int numInterpolations = (int)maxDis * precisionCounter;

                for (int j = 1; j <= numInterpolations; j++)
                {
                    float t = j / (float)(numInterpolations + 1);
                    Vector2 interpolatedValue = Vector2.Lerp(currentValue, nextValue, t);
                    interpolatedList.Add(interpolatedValue);
                }
            }

            interpolatedList.Add(originalList[originalList.Count - 1]);

            return interpolatedList;
        }

        /// <summary>
        /// 在给定的 Vector2 列表之间使用贝塞尔曲线进行平滑插值
        /// </summary>
        /// <param name="originalList">原始的 Vector2 列表</param>
        /// <param name="precision">插值精度，不能为零或负数</param>
        /// <returns>包含平滑插值点的新 Vector2 列表</returns>
        public static List<Vector2> InterpolateVectorListWithBezier(List<Vector2> originalList, float precision = 1)
        {
            if (precision <= 0) precision = 1;
            int precisionCounter = (int)(1f / precision);
            if (precisionCounter < 1) precisionCounter = 1;

            List<Vector2> interpolatedList = new List<Vector2>();

            for (int i = 0; i < originalList.Count - 1; i++)
            {
                Vector2 startPoint = originalList[i];
                Vector2 endPoint = originalList[i + 1];

                // 创建贝塞尔曲线的控制点，这里使用中点
                Vector2 controlPoint1 = (startPoint + endPoint) / 2;
                Vector2 controlPoint2 = (startPoint + endPoint) / 2;

                for (int j = 0; j <= precisionCounter; j++)
                {
                    float t = j / (float)precisionCounter;

                    // 计算贝塞尔曲线点
                    Vector2 interpolatedValue = CalculateBezierPoint(startPoint, controlPoint1, controlPoint2, endPoint, t);
                    interpolatedList.Add(interpolatedValue);
                }
            }

            interpolatedList.Add(originalList[originalList.Count - 1]);

            return interpolatedList;
        }

        // 计算贝塞尔曲线上的点
        private static Vector2 CalculateBezierPoint(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttu = tt * u;
            float ttt = tt * t;

            Vector2 p = uuu * p0;
            p += 3 * uu * t * p1;
            p += 3 * u * tt * p2;
            p += ttt * p3;

            return p;
        }
    }
}
