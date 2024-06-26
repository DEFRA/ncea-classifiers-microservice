﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace Ncea.Classifier.Microservice.Models;

public class FilterCriteria
{   
    [JsonRequired]
    [BindRequired]
    public int Level { get; set; }

    public string? Parents { get; set; }
}
