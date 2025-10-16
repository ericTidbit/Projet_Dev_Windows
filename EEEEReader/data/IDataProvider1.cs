using EEEEReader.Models;
using System.Collections.Generic;

public interface IDataProvider
{
    List<Data> GetData();
}