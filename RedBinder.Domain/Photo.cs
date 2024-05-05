using System.Data.Common;

namespace RedBinder.Domain;

public class Photo
{
    
    
    public int Id { get; }
    public string FilePath { get; }
    
    // TODO: get the photo using a file path, or would that be an infrastructure thing??
}