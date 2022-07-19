﻿using System;

namespace ServiceCollectionGenerators.Attributes
{
    /// <summary>
    ///     Attribute to be used on Options classes that should be added to the dependency injection container.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal class OptionsAttribute : Attribute
    {
        public string ConfigurationSectionName { get; set; }

        public bool ValidateDataAnnotations { get; set; }
    }
}
