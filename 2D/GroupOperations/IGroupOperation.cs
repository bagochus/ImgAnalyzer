using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D.GroupOperations
{
    public interface IGroupOperation
    {
        void Execute();
        string Description { get; }
        double[] SingleValueParameters { get; set; }
        string[] SingleValueNames { get; }
        IContainer_2D[] ContainerParameters { get; set; }
        string[] ContainerNames { get; }

        IImageSource[] imageSources { get; set; }
        string[] imageSourceNames { get;  }

        bool UseTransformation { get; set; }


    }
}
