using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MoneyKepper2.Behaviors
{
    public class SetDateToDatePickerBehavior : DependencyObject, IBehavior
    {
        public DependencyObject AssociatedObject
        {
            get; set;
        }

        public DateTimeOffset? Date
        {
            get { return (DateTimeOffset)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static readonly DependencyProperty DateProperty =
          DependencyProperty.Register("Date", typeof(DateTimeOffset), typeof(SetDateToDatePickerBehavior), new PropertyMetadata(null, OnDateChanged));

        private static void OnDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as SetDateToDatePickerBehavior;
            if (view.AssociatedObject is DatePicker)
            {
                ((DatePicker)(view.AssociatedObject)).Date = view.Date.Value;
            }
            else
            {
                ((CalendarDatePicker)(view.AssociatedObject)).Date = view.Date.Value;
            }
        }

        public void Attach(DependencyObject associatedObject)
        {
            this.AssociatedObject = associatedObject;
            if (AssociatedObject is DatePicker)
            {
                ((DatePicker)this.AssociatedObject).DateChanged += SetDateToDatePickerBehavior_DateChanged1;
            }
            else
            {
                ((CalendarDatePicker)this.AssociatedObject).DateChanged += SetDateToDatePickerBehavior_DateChanged;
            }
        }

        private void SetDateToDatePickerBehavior_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            if (((CalendarDatePicker)AssociatedObject).Date == null)
                return;

            this.Date = ((CalendarDatePicker)AssociatedObject).Date;
        }

        private void SetDateToDatePickerBehavior_DateChanged1(object sender, DatePickerValueChangedEventArgs e)
        {
            this.Date = ((DatePicker)AssociatedObject).Date.Date;
        }

        public void Detach()
        {
            if (AssociatedObject is DatePicker)
            {
                ((DatePicker)this.AssociatedObject).DateChanged -= SetDateToDatePickerBehavior_DateChanged1;
            }
            else
            {
                ((CalendarDatePicker)this.AssociatedObject).DateChanged += SetDateToDatePickerBehavior_DateChanged;
            }
        }
    }
}
