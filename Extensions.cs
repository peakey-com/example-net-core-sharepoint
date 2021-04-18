namespace console_push_data
{
    public static class SampleExtensions
    {


        public static double CelciusToFahrenheit(this double celcius)
        {
            //Fahrenheit = (Celcius × 9/5) + 32
            return (celcius * 9.0 / 5.0) + 32;
        }


    }
}