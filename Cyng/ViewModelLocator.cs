using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Cyng
{
    internal class ViewModelLocator
    {
        private static readonly List<object> ViewModelsList = new List<object>();

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.RegisterAttached("AutoViewModel", typeof(bool),
                typeof(ViewModelLocator), new PropertyMetadata(false, ViewModelChanged));

        public static bool GetViewModel(DependencyObject obj)
            => (bool)obj.GetValue(ViewModelProperty);

        public static void SetViewModel(DependencyObject obj, bool value)
            => obj.SetValue(ViewModelProperty, value);

        private static void ViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d)) return;

            var fullName = d.GetType().FullName;

            if (fullName == null) return;

            var viewModelType =
                Type.GetType($"{fullName.Replace(".Views.", ".ViewModels.")}Model");

            if (viewModelType == null) return;

            if (ViewModelsList.FirstOrDefault(x => x.GetType() == viewModelType) == null)
            {
                var viewModel = Activator.CreateInstance(viewModelType);
                ViewModelsList.Add(viewModel);
            }

            ((FrameworkElement)d).DataContext =
                ViewModelsList.FirstOrDefault(x => x.GetType() == viewModelType);
        }

        public static object Get(Type viewModelType)
        {
            if (ViewModelsList.FirstOrDefault(x => x.GetType() == viewModelType) == null)
            {
                var viewModel = Activator.CreateInstance(viewModelType);
                ViewModelsList.Add(viewModel);
            }

            return ViewModelsList.FirstOrDefault(x => x.GetType() == viewModelType);
        }
    }
}