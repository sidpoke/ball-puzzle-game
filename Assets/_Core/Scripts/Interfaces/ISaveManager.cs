/// <summary>
/// Save Manager Interface that implements the basic methods to save key-value pairs
/// </summary>
public interface ISaveManager
{
    public void SaveString(string key, string value);
    public string LoadString(string key);
    public void SaveInt(string key, int value);
    public int LoadInt(string key);
    public void Delete(string key);
}