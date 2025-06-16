namespace ServerWatchTower.Agent.View
{
    using ServerWatchTower.Agent.ViewModel;
    using System;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using Telerik.Windows.Controls.Charting;

    /// <summary>
    /// The view of Diagrams. 
    /// </summary>
    [Export("ServerWatchTower.Agent.DiagramsView"), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class DiagramsView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramsView"/> class.
        /// </summary>
        public DiagramsView()
        {
            this.InitializeComponent();

            this.DataContextChanged += (s, e) =>
            {
                if (e.NewValue is DiagramsViewModel vm)
                {
                    vm.UpdateChartCallback = groupedData =>
                    {
                        RadChartDiagram.DefaultView.ChartTitle.Content = "Server Diagrams";
                        RadChartDiagram.DefaultView.ChartLegend.Visibility = Visibility.Visible;
                        RadChartDiagram.SeriesMappings.Clear();

                        int colorIndex = 0;
                        Brush brush;

                        foreach (var group in groupedData)
                        {
                            brush = GenerateBrush(colorIndex++);

                            var mapping = new SeriesMapping
                            {
                                LegendLabel = group.Key,
                                ItemsSource = group.ToList(),
                                SeriesDefinition = new LineSeriesDefinition
                                {
                                    ShowItemLabels = true,
                                    EmptyPointBehavior = EmptyPointBehavior.Gap,
                                    Appearance =
                                    {
                                        Fill = brush,
                                        Stroke = brush,
                                        StrokeThickness = 1
                                    }
                                },
                            };

                            mapping.ItemMappings.Add(new ItemMapping("Label", DataPointMember.XCategory));
                            mapping.ItemMappings.Add(new ItemMapping("Value", DataPointMember.YValue));

                            RadChartDiagram.SeriesMappings.Add(mapping);
                        }
                    };
                }
            };
        }

        Brush GenerateBrush(int index)
        {
            double hue = (index * 137.508) % 360; // golden angle for max distribution
            var color = ColorFromHSV(hue, 0.6, 0.9);
            return new SolidColorBrush(color);
        }

        Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value *= 255;
            byte v = (byte)value;
            byte p = (byte)(value * (1 - saturation));
            byte q = (byte)(value * (1 - f * saturation));
            byte t = (byte)(value * (1 - (1 - f) * saturation));

            switch (hi)
            {
                case 0: return Color.FromRgb(v, t, p);
                case 1: return Color.FromRgb(q, v, p);
                case 2: return Color.FromRgb(p, v, t);
                case 3: return Color.FromRgb(p, q, v);
                case 4: return Color.FromRgb(t, p, v);
                default: return Color.FromRgb(v, p, q);
            }
        }
    }
}
