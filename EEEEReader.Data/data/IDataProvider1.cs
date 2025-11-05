using EEEEReader.Data.Models;
using System.Collections.Generic;

public interface IDataProvider
{
    List<Data> GetData();
}