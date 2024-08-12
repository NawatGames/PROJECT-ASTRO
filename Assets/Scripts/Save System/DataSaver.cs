[System.Serializable]
public class DataSaver
{
    private int _data;
    public DataSaver()
    {
        this._data = 1;
    }

    public int getData()
    {
        return this._data;
    }

    public void setData(int newData)
    {
        this._data = newData;
    }
}