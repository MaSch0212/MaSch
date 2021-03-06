namespace MaSch.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for value tuples.
    /// </summary>
    public static class ValueTupleExtensions
    {
        /// <summary>
        /// Expands the tuple as out parameters.
        /// </summary>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <typeparam name="TOut1">The type of the first out value.</typeparam>
        /// <param name="data">The value tuple to expand.</param>
        /// <param name="out1">The first out parameter.</param>
        /// <returns>The first element of the value tuple.</returns>
        public static TResult ExpandOut<TResult, TOut1>(this (TResult Result, TOut1 Out1) data, out TOut1 out1)
        {
            TResult result;
            (result, out1) = data;
            return result;
        }

        /// <summary>
        /// Expands the tuple as out parameters.
        /// </summary>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <typeparam name="TOut1">The type of the first out value.</typeparam>
        /// <typeparam name="TOut2">The type of the second out value.</typeparam>
        /// <param name="data">The value tuple to expand.</param>
        /// <param name="out1">The first out parameter.</param>
        /// <param name="out2">The second out parameter.</param>
        /// <returns>The first element of the value tuple.</returns>
        public static TResult ExpandOut<TResult, TOut1, TOut2>(this (TResult Result, TOut1 Out1, TOut2 Out2) data, out TOut1 out1, out TOut2 out2)
        {
            TResult result;
            (result, out1, out2) = data;
            return result;
        }

        /// <summary>
        /// Expands the tuple as out parameters.
        /// </summary>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <typeparam name="TOut1">The type of the first out value.</typeparam>
        /// <typeparam name="TOut2">The type of the second out value.</typeparam>
        /// <typeparam name="TOut3">The type of the third out value.</typeparam>
        /// <param name="data">The value tuple to expand.</param>
        /// <param name="out1">The first out parameter.</param>
        /// <param name="out2">The second out parameter.</param>
        /// <param name="out3">The third out parameter.</param>
        /// <returns>The first element of the value tuple.</returns>
        public static TResult ExpandOut<TResult, TOut1, TOut2, TOut3>(this (TResult Result, TOut1 Out1, TOut2 Out2, TOut3 Out3) data, out TOut1 out1, out TOut2 out2, out TOut3 out3)
        {
            TResult result;
            (result, out1, out2, out3) = data;
            return result;
        }

        /// <summary>
        /// Expands the tuple as out parameters.
        /// </summary>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <typeparam name="TOut1">The type of the first out value.</typeparam>
        /// <typeparam name="TOut2">The type of the second out value.</typeparam>
        /// <typeparam name="TOut3">The type of the third out value.</typeparam>
        /// <typeparam name="TOut4">The type of the fourth out value.</typeparam>
        /// <param name="data">The value tuple to expand.</param>
        /// <param name="out1">The first out parameter.</param>
        /// <param name="out2">The second out parameter.</param>
        /// <param name="out3">The third out parameter.</param>
        /// <param name="out4">The fourth out parameter.</param>
        /// <returns>The first element of the value tuple.</returns>
        public static TResult ExpandOut<TResult, TOut1, TOut2, TOut3, TOut4>(this (TResult Result, TOut1 Out1, TOut2 Out2, TOut3 Out3, TOut4 Out4) data, out TOut1 out1, out TOut2 out2, out TOut3 out3, out TOut4 out4)
        {
            TResult result;
            (result, out1, out2, out3, out4) = data;
            return result;
        }

        /// <summary>
        /// Expands the tuple as out parameters.
        /// </summary>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <typeparam name="TOut1">The type of the first out value.</typeparam>
        /// <typeparam name="TOut2">The type of the second out value.</typeparam>
        /// <typeparam name="TOut3">The type of the third out value.</typeparam>
        /// <typeparam name="TOut4">The type of the fourth out value.</typeparam>
        /// <typeparam name="TOut5">The type of the fifth out value.</typeparam>
        /// <param name="data">The value tuple to expand.</param>
        /// <param name="out1">The first out parameter.</param>
        /// <param name="out2">The second out parameter.</param>
        /// <param name="out3">The third out parameter.</param>
        /// <param name="out4">The fourth out parameter.</param>
        /// <param name="out5">The fifth out parameter.</param>
        /// <returns>The first element of the value tuple.</returns>
        public static TResult ExpandOut<TResult, TOut1, TOut2, TOut3, TOut4, TOut5>(this (TResult Result, TOut1 Out1, TOut2 Out2, TOut3 Out3, TOut4 Out4, TOut5 Out5) data, out TOut1 out1, out TOut2 out2, out TOut3 out3, out TOut4 out4, out TOut5 out5)
        {
            TResult result;
            (result, out1, out2, out3, out4, out5) = data;
            return result;
        }

        /// <summary>
        /// Expands the tuple as out parameters.
        /// </summary>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <typeparam name="TOut1">The type of the first out value.</typeparam>
        /// <typeparam name="TOut2">The type of the second out value.</typeparam>
        /// <typeparam name="TOut3">The type of the third out value.</typeparam>
        /// <typeparam name="TOut4">The type of the fourth out value.</typeparam>
        /// <typeparam name="TOut5">The type of the fifth out value.</typeparam>
        /// <typeparam name="TOut6">The type of the sixth out value.</typeparam>
        /// <param name="data">The value tuple to expand.</param>
        /// <param name="out1">The first out parameter.</param>
        /// <param name="out2">The second out parameter.</param>
        /// <param name="out3">The third out parameter.</param>
        /// <param name="out4">The fourth out parameter.</param>
        /// <param name="out5">The fifth out parameter.</param>
        /// <param name="out6">The sixth out parameter.</param>
        /// <returns>The first element of the value tuple.</returns>
        public static TResult ExpandOut<TResult, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(this (TResult Result, TOut1 Out1, TOut2 Out2, TOut3 Out3, TOut4 Out4, TOut5 Out5, TOut6 Out6) data, out TOut1 out1, out TOut2 out2, out TOut3 out3, out TOut4 out4, out TOut5 out5, out TOut6 out6)
        {
            TResult result;
            (result, out1, out2, out3, out4, out5, out6) = data;
            return result;
        }
    }
}
