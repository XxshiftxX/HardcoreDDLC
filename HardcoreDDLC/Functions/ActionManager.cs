using HardcoreDDLC.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace HardcoreDDLC
{
    public partial class MainWindow
    {
        private DispatcherTimer Animater = new DispatcherTimer();
        private List<DDLCAction> actions = new List<DDLCAction>();
        private int actionIndex = 0;
        
        private void BackgroundButton_Click(object sender, RoutedEventArgs e)
        {
            if (actions.Count <= actionIndex)
                return;

            switch (actions[actionIndex])
            {
                case DDLCScriptAction action:
                    if (!IsScriptRunning)
                    {
                        RegistScript(action.Script);
                    }
                    else
                    {
                        FinishScript();
                    }
                    break;
                case DDLCMoveAction action when action.Speed == 0:
                    action.Object.SetValue(Canvas.LeftProperty, (double)action.Object.GetValue(Canvas.LeftProperty) + action.Position.X);
                    action.Object.SetValue(Canvas.TopProperty, (double)action.Object.GetValue(Canvas.TopProperty) + action.Position.Y);
                    actionIndex++;
                    break;
                case DDLCMoveAction action when !action.Active:
                    action.Active = true;
                    ExecuteMove(action);
                    break;
                case DDLCKeyinputAction action:
                    var keys = action.Input.Split('|');

                    foreach(var key in keys)
                        AutoIt.AutoItX.Send(key);

                    actionIndex++;
                    break;
                case DDLCProcessAction action:
                    Process.Start(action.Path);
                    actionIndex++;
                    break;
            }

            if (actionIndex > 0 && actions.Count > actionIndex && actions[actionIndex-1].isSkiped)
                BackgroundButton_Click(null, null);
        }

        private void ExecuteMove(DDLCMoveAction action)
        {
            Animater = new DispatcherTimer
            {   
                Interval = TimeSpan.FromMilliseconds(10)
            };

            double originX = (double)action.Object.GetValue(Canvas.LeftProperty);
            double originY = (double)action.Object.GetValue(Canvas.TopProperty);
            double speedX = action.Position.X / 100 / action.Speed;
            double speedY = action.Position.Y / 100 / action.Speed;
            int count = 0;

            Animater.Tick += (x, y) =>
            {
                if (count++ > action.Speed * 100 - 1)
                {
                    Monika.SetValue(Canvas.LeftProperty, originX + action.Position.X);
                    Monika.SetValue(Canvas.TopProperty, originY + action.Position.Y);
                    actionIndex++;
                    Animater.Stop();
                    return;
                }

                action.Object.SetValue(Canvas.LeftProperty, (double)action.Object.GetValue(Canvas.LeftProperty) + speedX);
                action.Object.SetValue(Canvas.TopProperty, (double)action.Object.GetValue(Canvas.TopProperty) + speedY);
            };
            Animater.Start();
        }
    }
}