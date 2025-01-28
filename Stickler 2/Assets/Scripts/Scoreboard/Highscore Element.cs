using System;

[Serializable]
public class HighscoreElement
{
    public string playerName;
    public int waves;

    public HighscoreElement(string name, int waves)
    {
        playerName = name;
        this.waves = waves;
    }
}
