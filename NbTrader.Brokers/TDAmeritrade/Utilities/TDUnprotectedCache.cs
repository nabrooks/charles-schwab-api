using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace NbTrader.Brokers.TDAmeritrade.Utilities
{
    public class TDCache
    {
        private string _root;

        public TDCache(string root = "")
        {
            _root = root;
            var keyfile = new FileInfo("TDAmeritradeKey");
        }

        private Either<Error, Unit> Ensure(string path)
        {
            return Try(() =>
            {
                if (!File.Exists(path))
                {
                    using (var s = File.Create(path)) { }
                }
                return Unit.Default;
            })
                .ToEither(Fail: ex => Error.New(ex));
        }
       
        public Either<Error, string> Load(string key)
        {
            var path = Path.Combine(_root, key);
            return Ensure(path)
                .Map((unit) => File.ReadAllText(path));
        }

        public Either<Error, Unit> Save(string key, string value)
        {
            lock (this)
            {
                var path = Path.Combine(_root, key);
                return Ensure(path)
                    .Map((unit) => { File.WriteAllText(key, value); return Unit.Default; });
            }
        }
    }

    /// <summary>
    /// Saves security token as an UNPROTECTED FILE
    /// DO NOT USE IN PRODUCTION
    /// </summary>
    public class TDUnprotectedCache : ITdPersistentCache
    {
        private string _root;

        public TDUnprotectedCache(string root = "")
        {
            _root = root;
            var keyfile = new FileInfo("TDAmeritradeKey");
        }

        private void Ensure(string path)
        {
            if (!File.Exists(path))
            {
                using (var s = File.Create(path)) { }
            }
        }
        public string Load(string key)
        {
            lock (this)
            {
                var path = Path.Combine(_root, key);
                Ensure(path);
                return File.ReadAllText(path);
            }
        }
        public void Save(string key, string value)
        {
            lock (this)
            {
                var path = Path.Combine(_root, key);
                Ensure(path);
                File.WriteAllText(key, value);
            }
        }
    }
}
