using HardcoreDDLC.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace HardcoreDDLC
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _animater = new DispatcherTimer();
        private readonly List<DDLCAction> _actions = new List<DDLCAction>();
        private int _actionIndex = 0;
        private int _scriptIndex = 1;

        private void BackgroundButton_Click(object sender, RoutedEventArgs e)
        {
            while (true)
            {
                if (_actions.Count <= _actionIndex) return;

                var isSkipped = _actions[_actionIndex].isSkiped;

                switch (_actions[_actionIndex])
                {
                    case DDLCScriptAction action:
                        if (!_isScriptRunning)
                            RegistScript(action.Script);
                        else
                            FinishScript();

                        break;
                    case DDLCMoveAction action when action.Speed == 0:
                        action.Object.SetValue(Canvas.LeftProperty, (double) action.Object.GetValue(Canvas.LeftProperty) + action.Position.X);
                        action.Object.SetValue(Canvas.TopProperty, (double) action.Object.GetValue(Canvas.TopProperty) + action.Position.Y);
                        _actionIndex++;
                        break;
                    case DDLCMoveAction action when !action.Active:
                        action.Active = true;
                        ExecuteMove(action);
                        break;
                    case DDLCKeyinputAction action:
                        var keys = action.Input.Split('|');

                        foreach (var key in keys) AutoIt.AutoItX.Send(key);

                        _actionIndex++;
                        break;
                    case DDLCProcessAction action:
                        Process.Start(action.Path);
                        _actionIndex++;
                        break;
                    case DDLCDelayAction action when action.isSkiped:
                        new Thread(() =>
                        {
                            Thread.Sleep(action.Time);
                            _actionIndex++;
                            BackgroundButton_Click(null, null);
                        }).Start();
                        return;
                    case DDLCDelayAction action:
                        new Thread(() =>
                        {
                            Thread.Sleep(action.Time);
                            _actionIndex++;
                        }).Start();
                        break;
                }

                if (isSkipped)
                {
                    continue;
                }

                break;
            }
        }

        private void ExecuteMove(DDLCMoveAction action)
        {
            _animater = new DispatcherTimer
            {   
                Interval = TimeSpan.FromMilliseconds(10)
            };

            var originX = (double)action.Object.GetValue(Canvas.LeftProperty);
            var originY = (double)action.Object.GetValue(Canvas.TopProperty);
            var speedX = action.Position.X / 100 / action.Speed;
            var speedY = action.Position.Y / 100 / action.Speed;
            var count = 0;

            _animater.Tick += (x, y) =>
            {
                if (count++ > action.Speed * 100 - 1)
                {
                    Monika.SetValue(Canvas.LeftProperty, originX + action.Position.X);
                    Monika.SetValue(Canvas.TopProperty, originY + action.Position.Y);
                    _actionIndex++;
                    _animater.Stop();
                    return;
                }

                action.Object.SetValue(Canvas.LeftProperty, (double)action.Object.GetValue(Canvas.LeftProperty) + speedX);
                action.Object.SetValue(Canvas.TopProperty, (double)action.Object.GetValue(Canvas.TopProperty) + speedY);
            };
            _animater.Start();
        }
    }
}