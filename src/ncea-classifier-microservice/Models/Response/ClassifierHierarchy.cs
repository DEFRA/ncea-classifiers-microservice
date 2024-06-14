﻿namespace Ncea.Classifier.Microservice.Models
{
    public class ClassifierHierarchy
    {        
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public List<ClassifierHierarchy> Classifiers { get; set; } = [];
    }
}