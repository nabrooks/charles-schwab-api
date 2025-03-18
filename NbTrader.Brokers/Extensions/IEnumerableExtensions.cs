namespace NbTrader.Brokers.Extensions
{
    /// <summary>
    /// IEnumerable extensions
    /// </summary>
    public static class IEnumerableExtensions
    {
        #region Functions

        #region Concat

        /// <summary>
        /// Combines multiple IEnumerables together and returns a new IEnumerable containing all of the values
        /// </summary>
        /// <typeparam name="T">Type of the data in the IEnumerable</typeparam>
        /// <param name="Enumerable1">IEnumerable 1</param>
        /// <param name="Additions">IEnumerables to concat onto the first item</param>
        /// <returns>A new IEnumerable containing all values</returns>
        /// <example>
        /// <code>
        ///  int[] TestObject1 = new int[] { 1, 2, 3 };
        ///  int[] TestObject2 = new int[] { 4, 5, 6 };
        ///  int[] TestObject3 = new int[] { 7, 8, 9 };
        ///  TestObject1 = TestObject1.Concat(TestObject2, TestObject3).ToArray();
        /// </code>
        /// </example>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> Enumerable1, params IEnumerable<T>[] Additions)
        {
            List<T> Results = new List<T>();
            Results.AddRange(Enumerable1);
            for (int x = 0; x < Additions.Length; ++x)
                Results.AddRange(Additions[x]);
            return Results;
        }

        #endregion

        #region Distinct

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element)));
        }

        #endregion Distinct

        #region ElementsBetween

        /// <summary>
        /// Returns elements starting at the index and ending at the end index
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">List to search</param>
        /// <param name="Start">Start index (inclusive)</param>
        /// <param name="End">End index (exclusive)</param>
        /// <returns>The items between the start and end index</returns>
        public static IEnumerable<T> ElementsBetween<T>(this IEnumerable<T> List, int Start, int End)
        { 
            if (End > List.Count())
                End = List.Count();
            if (Start < 0)
                Start = 0;
            System.Collections.Generic.List<T> ReturnList = new System.Collections.Generic.List<T>();
            for (int x = Start; x < End; ++x)
                ReturnList.Add(List.ElementAt(x));
            return ReturnList;
        }

        #endregion

        #region For

        /// <summary>
        /// Does an action for each item in the IEnumerable between the start and end indexes
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Start">Item to start with</param>
        /// <param name="End">Item to end with</param>
        /// <param name="Action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> For<T>(this IEnumerable<T> List, int Start, int End, Action<T> Action)
        {
            foreach (T Item in List.ElementsBetween(Start, End + 1))
                Action(Item);
            return List;
        }

        /// <summary>
        /// Does a function for each item in the IEnumerable between the start and end indexes and returns an IEnumerable of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Start">Item to start with</param>
        /// <param name="End">Item to end with</param>
        /// <param name="Function">Function to do</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> For<T, R>(this IEnumerable<T> List, int Start, int End, Func<T, R> Function)
        {
            List<R> ReturnValues = new List<R>();
            foreach (T Item in List.ElementsBetween(Start, End + 1))
                ReturnValues.Add(Function(Item));
            return ReturnValues;
        }

        #endregion

        #region ForEach

        /// <summary>
        /// Does an action for each item in the IEnumerable
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> List, Action<T> Action)
        {
            foreach (T Item in List)
                Action(Item);
            return List;
        }

        /// <summary>
        /// Does a function for each item in the IEnumerable, returning a list of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Function">Function to do</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> ForEach<T, R>(this IEnumerable<T> List, Func<T, R> Function)
        {
            List<R> ReturnValues = new List<R>();
            foreach (T Item in List)
                ReturnValues.Add(Function(Item));
            return ReturnValues;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Action">Action to do</param>
        /// <param name="CatchAction">Action that occurs if an exception occurs</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> List, Action<T> Action, Action<T, Exception> CatchAction)
        {
            foreach (T Item in List)
            {
                try
                {
                    Action(Item);
                }
                catch (Exception e) { CatchAction(Item, e); }
            }
            return List;
        }

        /// <summary>
        /// Does a function for each item in the IEnumerable, returning a list of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Function">Function to do</param>
        /// <param name="CatchAction">Action that occurs if an exception occurs</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> ForEach<T, R>(this IEnumerable<T> List, Func<T, R> Function, Action<T, Exception> CatchAction)
        {
            List<R> ReturnValues = new List<R>();
            foreach (T Item in List)
            {
                try
                {
                    ReturnValues.Add(Function(Item));
                }
                catch (Exception e) { CatchAction(Item, e); }
            }
            return ReturnValues;
        }

        #endregion

        #region ForParallel

        /// <summary>
        /// Does an action for each item in the IEnumerable between the start and end indexes in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Start">Item to start with</param>
        /// <param name="End">Item to end with</param>
        /// <param name="Action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForParallel<T>(this IEnumerable<T> List, int Start, int End, Action<T> Action)
        {
            Parallel.For(Start, End + 1, new Action<int>(x => Action(List.ElementAt(x))));
            return List;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable between the start and end indexes in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Results type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Start">Item to start with</param>
        /// <param name="End">Item to end with</param>
        /// <param name="Function">Function to do</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> ForParallel<T, R>(this IEnumerable<T> List, int Start, int End, Func<T, R> Function)
        {
            R[] Results = new R[(End + 1) - Start];
            Parallel.For(Start, End + 1, new Action<int>(x => Results[x - Start] = Function(List.ElementAt(x))));
            return Results;
        }

        #endregion

        #region ForEachParallel

        /// <summary>
        /// Does an action for each item in the IEnumerable in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEachParallel<T>(this IEnumerable<T> List, Action<T> Action)
        {
            Parallel.ForEach(List, Action);
            return List;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Results type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Function">Function to do</param>
        /// <returns>The results in an IEnumerable list</returns>
        public static IEnumerable<R> ForEachParallel<T, R>(this IEnumerable<T> List, Func<T, R> Function)
        {
            return List.ForParallel(0, List.Count() - 1, Function);
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Action">Action to do</param>
        /// <param name="CatchAction">Action that occurs if an exception occurs</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEachParallel<T>(this IEnumerable<T> List, Action<T> Action, Action<T, Exception> CatchAction)
        {
            Parallel.ForEach<T>(List, delegate (T Item)
            {
                try
                {
                    Action(Item);
                }
                catch (Exception e) { CatchAction(Item, e); }
            });
            return List;
        }

        /// <summary>
        /// Does a function for each item in the IEnumerable, returning a list of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Function">Function to do</param>
        /// <param name="CatchAction">Action that occurs if an exception occurs</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> ForEachParallel<T, R>(this IEnumerable<T> List, Func<T, R> Function, Action<T, Exception> CatchAction)
        {
            List<R> ReturnValues = new List<R>();
            Parallel.ForEach<T>(List, delegate (T Item)
            {
                try
                {
                    ReturnValues.Add(Function(Item));
                }
                catch (Exception e) { CatchAction(Item, e); }
            });
            return ReturnValues;
        }

        #endregion

        #region Last

        /// <summary>
        /// Returns the last X number of items from the list
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Count">Numbers of items to return</param>
        /// <returns>The last X items from the list</returns>
        public static IEnumerable<T> Last<T>(this IEnumerable<T> List, int Count)
        {
            return List.ElementsBetween(List.Count() - Count, List.Count());
        }

        #endregion

        #region MostCommon

        /// <summary>
        /// Gets the most common element from a collection
        /// </summary>
        /// <typeparam name="T">The type to evaluate</typeparam>
        /// <param name="list">The collection to operate on</param>
        /// <returns>The most common element</returns>
        public static T MostCommon<T>(this IEnumerable<T> list)
        {
            var most = list.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
            return most;
        }

        #endregion

        #region Median

        /// <summary>
        /// Returns the element at the middle index of the collection if the collection count is odd, else will return the middle index + 1 element of the collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T Median<T>(this IEnumerable<T> source)
        {
            if (Nullable.GetUnderlyingType(typeof(T)) != null)
                source = source.Where(x => x != null);

            int count = source.Count();
            if (count == 0)
                throw new Exception("There are no elements in the collection.");

            source = source.OrderBy(n => n);

            int midpoint = count / 2;
            if (count % 2 == 0)
                return source.ElementAt(midpoint - 1);
            else
                return source.ElementAt(midpoint);
        }

        #endregion
          
        #region Remove

        /// <summary>
        /// Removes values from a list that meet the criteria set forth by the predicate
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="Value">List to cull items from</param>
        /// <param name="Predicate">Predicate that determines what items to remove</param>
        /// <returns>An IEnumerable with the objects that meet the criteria removed</returns>
        public static IEnumerable<T> Remove<T>(this IEnumerable<T> Value, Func<T, bool> Predicate)
        {
            return Value.Where(x => !Predicate(x));
        }

        #endregion

        #region ToArray

        /// <summary>
        /// Converts a list to an array
        /// </summary>
        /// <typeparam name="Source">Source type</typeparam>
        /// <typeparam name="Target">Target type</typeparam>
        /// <param name="List">List to convert</param>
        /// <param name="ConvertingFunction">Function used to convert each item</param>
        /// <returns>The array containing the items from the list</returns>
        public static Target[] ToArray<Source, Target>(this IEnumerable<Source> List, Func<Source, Target> ConvertingFunction)
        {
            return List.ForEach(ConvertingFunction).ToArray();
        }

        #endregion

        #region ToList

        /// <summary>
        /// Converts an IEnumerable to a list
        /// </summary>
        /// <typeparam name="Source">Source type</typeparam>
        /// <typeparam name="Target">Target type</typeparam>
        /// <param name="List">IEnumerable to convert</param>
        /// <param name="ConvertingFunction">Function used to convert each item</param>
        /// <returns>The list containing the items from the IEnumerable</returns>
        public static List<Target> ToList<Source, Target>(this IEnumerable<Source> List, Func<Source, Target> ConvertingFunction)
        {
            return List.ForEach(ConvertingFunction).ToList();
        }

        #endregion

        #region ThrowIfAll

        /// <summary>
        /// Throws the specified exception if the predicate is true for all items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="List">The item</param>
        /// <param name="Predicate">Predicate to check</param>
        /// <param name="Exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static IEnumerable<T> ThrowIfAll<T>(this IEnumerable<T> List, Predicate<T> Predicate, Func<Exception> Exception)
        {
            foreach (T Item in List)
            {
                if (!Predicate(Item))
                    return List;
            }
            throw Exception();
        }

        /// <summary>
        /// Throws the specified exception if the predicate is true for all items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="List">The item</param>
        /// <param name="Predicate">Predicate to check</param>
        /// <param name="Exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static IEnumerable<T> ThrowIfAll<T>(this IEnumerable<T> List, Predicate<T> Predicate, Exception Exception)
        {
            foreach (T Item in List)
            {
                if (!Predicate(Item))
                    return List;
            }
            throw Exception;
        }

        #endregion

        #region ThrowIfAny

        /// <summary>
        /// Throws the specified exception if the predicate is true for any items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="List">The item</param>
        /// <param name="Predicate">Predicate to check</param>
        /// <param name="Exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static IEnumerable<T> ThrowIfAny<T>(this IEnumerable<T> List, Predicate<T> Predicate, Func<Exception> Exception)
        {
            foreach (T Item in List)
            {
                if (Predicate(Item))
                    throw Exception();
            }
            return List;
        }

        /// <summary>
        /// Throws the specified exception if the predicate is true for any items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="List">The item</param>
        /// <param name="Predicate">Predicate to check</param>
        /// <param name="Exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static IEnumerable<T> ThrowIfAny<T>(this IEnumerable<T> List, Predicate<T> Predicate, Exception Exception)
        {
            foreach (T Item in List)
            {
                if (Predicate(Item))
                    throw Exception;
            }
            return List;
        }

        #endregion

        #region Mean

        public static double Mean(this IEnumerable<double> values)
        {
            double sum = 0;
            int count = 0;

            foreach (double d in values)
            {
                sum += d;
                count++;
            }

            return sum / count;
        }

        public static float Mean(this IEnumerable<float> values)
        {
            float sum = 0;
            int count = 0;

            foreach (float d in values)
            {
                sum += d;
                count++;
            }

            return sum / count;
        }

        public static float Mean(this IEnumerable<int> values)
        {
            float sum = 0;
            int count = 0;

            foreach (float d in values)
            {
                sum += d;
                count++;
            }

            return sum / count;
        }

        #endregion Mean

        #region Standard Deviation

        public static double StandardDeviation(this IEnumerable<double> values, out double mean)
        {
            mean = values.Mean();
            double sumOfDiffSquares = 0;
            int count = 0;

            foreach (double d in values)
            {
                double diff = (d - mean);
                sumOfDiffSquares += diff * diff;
                count++;
            }

            return Math.Sqrt(sumOfDiffSquares / count);
        }

        public static float StandardDeviation(this IEnumerable<float> values, out float mean)
        {
            mean = values.Mean();
            float sumOfDiffSquares = 0;
            int count = 0;

            foreach (float d in values)
            {
                float diff = (d - mean);
                sumOfDiffSquares += diff * diff;
                count++;
            }

            return (float)Math.Sqrt(sumOfDiffSquares / count);
        }

        public static float StandardDeviation(this IEnumerable<int> values, out float mean)
        {
            mean = values.Mean();
            float sumOfDiffSquares = 0;
            int count = 0;

            foreach (float d in values)
            {
                float diff = (d - mean);
                sumOfDiffSquares += diff * diff;
                count++;
            }

            return (float)Math.Sqrt(sumOfDiffSquares / count);
        }

        #endregion Standard Deviation

        #endregion
    }

}
