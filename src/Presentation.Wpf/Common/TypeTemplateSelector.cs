using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace MaSch.Presentation.Wpf.Common
{
    [ContentProperty(nameof(DataTemplates))]
    public class TypeTemplateSelector : DataTemplateSelector
    {
        public List<DataTemplate> DataTemplates { get; set; }

        public TypeTemplateSelector()
        {
            DataTemplates = new List<DataTemplate>();
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            foreach (var dataTemplate in DataTemplates)
            {
                if ((dataTemplate.DataType as Type)?.IsInstanceOfType(item) == true)
                    return dataTemplate;
            }
            return DataTemplates.FirstOrDefault(x => x.DataType == null);
        }
    }
}
