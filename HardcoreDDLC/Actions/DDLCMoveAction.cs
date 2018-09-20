using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HardcoreDDLC.Actions
{
    class DDLCMoveAction : DDLCAction
    {
        public DependencyObject Object { get; set; }
        public double Speed { get; set; }
        public Point Position { get; set; }
        public bool Active { get; set; }

        public DDLCMoveAction(DependencyObject dependencyObject, Point pos, double speed = 0)
        {
            Object = dependencyObject;
            Position = pos;
            Speed = speed;
        }
    }
}
