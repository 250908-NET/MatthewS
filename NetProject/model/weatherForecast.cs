namespace NetProject.model;

public class WeatherForecast
{

    //fields
    public DateTime Date { get; set; }

    public int TempC { get; set; }

    public string Summary { get; set; }

    public int TempF { get; set; }

    //methods
    //constructors
    public WeatherForecast()
    {
        this.Date = DateTime.Now.Date;
        this.TempC = 0;
        this.Summary = "NOTHING";
        this.TempF = 0;
    }
    public WeatherForecast(DateTime Date, int TempC, string Summary)
    {
        this.Date = Date;
        this.TempC = TempC;
        this.Summary = Summary;
        this.TempF = (int)((TempC - 32) / 1.8);
    }
    public override string ToString()
    {
        return $"Date: {Date}, Temp(F): {TempF}, Summary: {Summary}, Temp(C): {TempC}";
    }
}