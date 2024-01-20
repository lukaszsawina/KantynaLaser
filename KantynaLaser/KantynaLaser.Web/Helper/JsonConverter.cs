using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace KantynaLaser.Web.Helper;

public class ListJsonConverter<T> : ValueConverter<List<T>, string>
{
    public ListJsonConverter() : base(
        v => JsonConvert.SerializeObject(v),
        v => JsonConvert.DeserializeObject<List<T>>(v))
    { }
}
