namespace Fanex.Age.UI
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Represent .net data in JSON format.
    /// </summary>
    public class JsonData : List<DictionaryEntry>
    {
        public JsonData()
        {
        }

        public JsonData(int capacity)
        {
            Capacity = capacity;
        }

        public void Add(object name, object value)
        {
            Add(new DictionaryEntry(name, value));
        }

        /// <summary>
        /// Return an error.
        /// </summary>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        public void AsError(int errCode, string errMsg)
        {
            Clear();
            Add(new DictionaryEntry(JsonError.ErrorCode, errCode));
            Add(new DictionaryEntry(JsonError.ErrorMsg, errMsg));
        }

        /// <summary>
        /// Return an error.
        /// </summary>
        /// <param name="errType"></param>
        /// <param name="errMsg"></param>
        public void AsError(ErrorType errType, string errMsg)
        {
            Clear();
            Add(new DictionaryEntry(JsonError.ErrorCode, (int)errType));
            Add(new DictionaryEntry(JsonError.ErrorMsg, errMsg));
        }

        /// <summary>
        /// Return a result.
        /// </summary>
        /// <param name="result"></param>
        public void AsResult(Result result)
        {
            Clear();
            Add(new DictionaryEntry(JsonError.ErrorCode, result.ErrCode));
            if (result.ErrCode != 0)
            {
                Add(new DictionaryEntry(JsonError.ErrorMsg, result.ErrMsg));
            }
        }

        /// <summary>
        /// Return a result.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="isMsgSuccess"></param>
        public void AsResult(Result result, bool isMsgSuccess)
        {
            Clear();
            Add(new DictionaryEntry(JsonError.ErrorCode, result.ErrCode));
            if (isMsgSuccess)
            {
                Add(new DictionaryEntry(JsonError.ErrorMsg, result.ErrMsg));
            }
        }

        /// <summary>
        /// Return a success result.
        /// </summary>
        public void AsSuccessResult()
        {
            Clear();
            Add(new DictionaryEntry(JsonError.ErrorCode, (int)ErrorType.Success));
        }
    }
}