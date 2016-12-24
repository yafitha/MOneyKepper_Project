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
    public class SetMinAndMaxDateBehavior : DependencyObject, IBehavior
    {
        public DependencyObject AssociatedObject
        {
            get; set;
        }

        public DateTime StartTime
        {
            get { return (DateTime)GetValue(StartTimeProperty); }
            set { SetValue(StartTimeProperty, value); }
        }

        public static readonly DependencyProperty StartTimeProperty =
            DependencyProperty.Register("StartTime", typeof(DateTime), typeof(SetMinAndMaxDateBehavior), new PropertyMetadata(null, OnStartTimeChanged));

        public DateTime EndTime
        {
            get { return (DateTime)GetValue(EndTimeProperty); }
            set { SetValue(EndTimeProperty, value); }
        }

        public static readonly DependencyProperty EndTimeProperty =
            DependencyProperty.Register("EndTime", typeof(DateTime), typeof(SetMinAndMaxDateBehavior), new PropertyMetadata(null, OnEndTimeChanged));

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
        }

        private static void OnStartTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (SetMinAndMaxDateBehavior)d;
            if ((view.AssociatedObject) is CalendarDatePicker)
            {
                ((CalendarDatePicker)view.AssociatedObject).MinDate = view.StartTime;
            }
        }

        private static void OnEndTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (SetMinAndMaxDateBehavior)d;
            if ((view.AssociatedObject) is CalendarDatePicker)
            {
                ((CalendarDatePicker)view.AssociatedObject).MaxDate = view.EndTime;
            }
        }
        public void Detach()
        {

        }
    }
}
