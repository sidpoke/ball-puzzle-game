public interface ISaveManager
{
    //public string SelectLevel(); <-- store ScriptableObject for Adventure Mode
    public void SaveString(string key, string value);
    public string LoadString(string key);
    public void SaveInt(string key, int value);
    public int LoadInt(string key);
    public void Delete(string key);
}