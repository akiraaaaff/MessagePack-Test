using MessagePack;
using System.Buffers;
using UnityEngine;
using UnityEngine.Profiling;

[MessagePackObject]
public struct Data
{
    [Key(0)]
    public int MyProperty1;
    [Key(1)]
    public int MyProperty2;
}

public class NewMonoBehaviourScript : MonoBehaviour
{
    Data data = new Data { MyProperty1 = 99, MyProperty2 = 9999 };
    ArrayBufferWriter<byte> writer = new ArrayBufferWriter<byte>();

    void Start()
    {
        MessagePackSerializer.Serialize(writer, data);
        Debug.LogWarning(MessagePackSerializer.ConvertToJson(writer.WrittenMemory));
        var data2 = MessagePackSerializer.Deserialize<Data>(writer.WrittenMemory);
        writer.Clear();
        Debug.LogWarning(data2);
    }

    private void Update()
    {
        参考UseCache();
    }

    void 参考()
    {
        Profiler.BeginSample(nameof(参考));
        for (int i = 0; i < 10000; i++)
        {
            var d = MessagePackSerializer.Serialize(data);
            var data2 = MessagePackSerializer.Deserialize<Data>(d);
        }
        Profiler.EndSample();
    }

    void 参考UseCache()
    {
        Profiler.BeginSample(nameof(参考UseCache));
        for (int i = 0; i < 10000; i++)
        {
            MessagePackSerializer.Serialize(writer, data);
            var data2 = MessagePackSerializer.Deserialize<Data>(writer.WrittenMemory);
            writer.Clear();
        }
        Profiler.EndSample();
    }
}
