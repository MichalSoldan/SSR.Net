using System;
using System.Collections.Generic;

namespace SSR.Net.Services
{
    public class JavaScriptEnginePoolConfig
    {
        public List<string> Scripts { get; private set; } = new List<string>();
        public Dictionary<string, Type> HostTypes { get; private set; } = new Dictionary<string, Type>();
        public Dictionary<string, object> HostObjects { get; private set; } = new Dictionary<string, object>();
        public int MaxEngines { get; private set; } = 25;
        public int MinEngines { get; private set; } = 5;
        public int MaxUsages { get; private set; } = 100;
        public int GarbageCollectionInterval { get; private set; } = 20;
        public int StandbyEngineCount { get; private set; } = 3;
        public int ReconfigureTimeoutMs { get; private set; } = 2000;

        public JavaScriptEnginePoolConfig AddHostType(string name, Type type)
        {
            HostTypes.Add(name, type);
            return this;
        }

        public JavaScriptEnginePoolConfig AddHostObject(string name, object obj)
        {
            HostObjects.Add(name, obj);
            return this;
        }

        public JavaScriptEnginePoolConfig AddScriptFile(string path)
        {
            string script = System.IO.File.ReadAllText(path);
            return AddScript(script);
        }

        public JavaScriptEnginePoolConfig AddScript(string script)
        {
            Scripts.Add(script);
            return this;
        }

        public JavaScriptEnginePoolConfig WithMaxEngineCount(int maxEngines)
        {
            MaxEngines = maxEngines;
            return this;
        }

        public JavaScriptEnginePoolConfig WithMinEngineCount(int minEngines)
        {
            MinEngines = minEngines;
            return this;
        }

        public JavaScriptEnginePoolConfig WithMaxUsagesCount(int maxUsages)
        {
            MaxUsages = maxUsages;
            return this;
        }

        public JavaScriptEnginePoolConfig WithGarbageCollectionInterval(int garbageCollectionInterval)
        {
            GarbageCollectionInterval = garbageCollectionInterval;
            return this;
        }

        public JavaScriptEnginePoolConfig WithStandbyEngineCount(int standbyEngineCount)
        {
            StandbyEngineCount = standbyEngineCount;
            return this;
        }

        public void Validate()
        {
            if (MinEngines <= 0)
                throw new InvalidOperationException($"{nameof(MinEngines)} must be a positive integer, but is set to {MinEngines}");
            if (MaxEngines < MinEngines)
                throw new InvalidOperationException($"{nameof(MaxEngines)} ({MaxEngines})must be at least as high as {nameof(MinEngines)} ({MinEngines})");
            if (StandbyEngineCount <= 0)
                throw new InvalidOperationException($"{nameof(StandbyEngineCount)} must be a positive integer, but is set to {StandbyEngineCount}");
            if (GarbageCollectionInterval <= 0)
                throw new InvalidOperationException($"{nameof(GarbageCollectionInterval)} must be a positive integer, but is set to {GarbageCollectionInterval}");
            if (MaxUsages <= 0)
                throw new InvalidOperationException($"{nameof(MaxUsages)} must be a positive integer, but is set to {MaxUsages}");
            if (ReconfigureTimeoutMs <= 0)
                throw new InvalidOperationException($"{nameof(ReconfigureTimeoutMs)} must be zero or higher, but is set to {ReconfigureTimeoutMs}");
        }
    }
}
