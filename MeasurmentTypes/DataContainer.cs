using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer.MeasurmentTypes
{
    public enum ImageStatus { Unprocessed = 0, InWork, Done, Other}
    public enum ContainerStatus { Unprocessed, InWork, Done, Obsolete}
    public class DataContainer : INotifyPropertyChanged
    {
        public IMeasurment measurment;
        public ImageStatus[] imageStatus;
        public double[] data;
        public ContainerStatus containerStatus { 
            get { return _status; }
            set 
            {
                if (_status != value)
                {
                    _status = value;
                    Status = _status.ToString();
                }
            } }
        private ContainerStatus _status;
        public string Name { get; set; }
        public string ImageGroup { get 
            {
                if (measurment == null) { return "---"; }
                if (measurment.Batch == null) { return "---"; }
                return ImageManager.GetIndexLabel(measurment.Batch);
            } }
        public string Type { get 
            {
                if (measurment == null) { return "Other"; }
                return measurment.GetType().Name;
            }}

        public string Status { get {  
                return _s_status; } 
            set
            {
                if (_s_status != value)
                {
                    _s_status = value;
                    OnPropertyChanged();
                }
            } }
        private string _s_status;


        public int DataCount { get { return dataCount; } }
        private int dataCount;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public DataContainer (IMeasurment measurment, ImageBatch imageBatch)
        {
            this.measurment = measurment;
            this.measurment.BindImageStack (imageBatch);
            data = new double[imageBatch.Count];
            imageStatus = new ImageStatus[imageBatch.Count];
            for (int i = 0; i < imageBatch.Count; i++) imageStatus[i] = ImageStatus.Unprocessed;
            dataCount = imageBatch.Count;
            Name = measurment.Name;
            _status = ContainerStatus.Unprocessed;
            _s_status = _status.ToString ();

        }





    }
}
