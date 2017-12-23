using System;
using GoldenSeries.Dros.Helpers;
using GoldenSeries.Dros.Models;
using GoldenSeries.Dros.Services;

namespace GoldenSeries.Dros
{
    public class App
    {

        public static void Initialize()
        {
            ServiceLocator.Instance.Register<IDataStore<Item>, MockDataStore>();
        }
    }
}
