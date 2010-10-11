using System;
using System.ComponentModel;

namespace TurboNH
{
	public class SelfTrackingEntity : INotifyPropertyChanged
	{
		private string description;
		public virtual Guid Id { get; set; }

		public virtual string Description
		{
			get { return description; }
			set
			{
				if (value != description)
				{
					description = value;
					NotifyPropertyChanged("Description");
				}
			}
		}

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		private void NotifyPropertyChanged(string info)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}
	}
}