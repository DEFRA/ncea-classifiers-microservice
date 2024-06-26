﻿namespace Ncea.Classifier.Microservice.Data.Entities;

public class Theme : ClassifierBaseEntity
{
    public List<Category> Categories { get; set; } = [];
    public List<SearchPageContent> SearchPageContentBlocks { get; set; } = [];
}
