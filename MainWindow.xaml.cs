using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Newtonsoft.Json;

namespace bucket_list
{
    public partial class MainWindow : Window
    {
        public string path;

        public MainWindow()
        {
            InitializeComponent();

            var directory = Directory.GetCurrentDirectory();

            if (!File.Exists(directory + @"\bucketlistitems.json"))
            {
                File.Create(directory + @"\bucketlistitems.json").Close();
                File.WriteAllText(directory + @"\bucketlistitems.json", "{\"items\":[]}");
            }

            path = directory + @"\bucketlistitems.json";
            LoadBucketListItem();
        }

        public void Minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        public void Shutdown(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        public void Drag(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        public void CreateBucketListItem(object sender, EventArgs e)
        {
            Grid grid = new Grid();
            grid.Height = 90;
            grid.VerticalAlignment = VerticalAlignment.Top;
            grid.Margin = new Thickness(5);

            Border border1 = new Border();
            border1.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#5BC65D");
            border1.BorderThickness = new Thickness(3);
            border1.CornerRadius = new CornerRadius(5);

            Label label = new Label();
            label.Content = "Bucket list item " + (bucketListStackPanel.Children.Count + 1);
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.VerticalAlignment = VerticalAlignment.Top;
            label.Margin = new Thickness(20,15,0,0);
            label.Width = 150;
            label.Height = 30;
            label.FontSize = 15;
            label.FontWeight = FontWeights.SemiBold;

            TextBox textBox = new TextBox();
            textBox.Text = string.Empty;
            textBox.HorizontalAlignment = HorizontalAlignment.Left;
            textBox.VerticalAlignment = VerticalAlignment.Top;
            textBox.Margin = new Thickness(25, 50,0,0);
            textBox.Width = 250;
            textBox.Height = 30;
            textBox.FontSize = 13;
            textBox.BorderThickness = new Thickness(0);

            Border border2 = new Border();
            border2.Width = 80;
            border2.Height = 30;
            border2.CornerRadius = new CornerRadius(5);
            border2.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#5BC65D");
            border2.BorderThickness = new Thickness(0);
            border2.HorizontalAlignment = HorizontalAlignment.Right;
            border2.Margin = new Thickness(0, 0, 40, 0);
            border2.PreviewMouseDown += SaveBucketListItem;

            TextBlock textBlock = new TextBlock();
            textBlock.Text = "Save";
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.FontWeight = FontWeights.SemiBold;
            textBlock.Foreground = Brushes.White;

            border2.Child = textBlock;

            grid.Children.Add(border1);
            grid.Children.Add(label);
            grid.Children.Add(textBox);
            grid.Children.Add(border2);

            bucketListStackPanel.Children.Add(grid);

            grid.BringIntoView();
        }
 
        public void SaveBucketListItem(object sender, EventArgs e)
        {
            Border border = (Border)sender;
            (border.Child as TextBlock).Text = "Delete";
            border.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FC7753");

            Grid grid = border.Parent as Grid;

            String text = (grid.Children[2] as TextBox).Text;

            grid.Children.Remove(grid.Children[2] as TextBox);

            TextBlock textBlock = new TextBlock();
            textBlock.Text = "• " + text;
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock.VerticalAlignment = VerticalAlignment.Top;
            textBlock.Margin = new Thickness(25, 50, 0, 0);
            textBlock.Width = 250;
            textBlock.Height = 30;
            textBlock.FontSize = 13;

            grid.Children.Add(textBlock);

            BucketListItems listItems = JsonConvert.DeserializeObject<BucketListItems>(File.ReadAllText(path));
            listItems.items.Add(text);
            File.WriteAllText(path, JsonConvert.SerializeObject(listItems));

            LoadBucketListItem();
        }

        public void DeleteBucketListItem(object sender, EventArgs e)
        {
            Grid grid = (Grid)(sender as Border).Parent;
            string text = (grid.Children[1] as Label).Content.ToString();
            int index = int.Parse(text[text.Length-1].ToString());

            BucketListItems listItems = JsonConvert.DeserializeObject<BucketListItems>(File.ReadAllText(path));
            try
            {
                listItems.items.RemoveAt(index - 1);
            }
            finally
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(listItems));
            }

            LoadBucketListItem();
        }

        private void LoadBucketListItem()
        {
            BucketListItems listItems = JsonConvert.DeserializeObject<BucketListItems>(File.ReadAllText(path));


            bucketListStackPanel.Children.Clear();
            foreach (string item in listItems.items)
            {
                Grid grid = new Grid();
                grid.Height = 90;
                grid.VerticalAlignment = VerticalAlignment.Top;
                grid.Margin = new Thickness(5);

                Border border1 = new Border();
                border1.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#5BC65D");
                border1.BorderThickness = new Thickness(3);
                border1.CornerRadius = new CornerRadius(5);

                Label label = new Label();
                label.Content = "Bucket list item " + (bucketListStackPanel.Children.Count + 1);
                label.HorizontalAlignment = HorizontalAlignment.Left;
                label.VerticalAlignment = VerticalAlignment.Top;
                label.Margin = new Thickness(20, 15, 0, 0);
                label.Width = 150;
                label.Height = 30;
                label.FontSize = 15;
                label.FontWeight = FontWeights.SemiBold;

                TextBlock textBlock1 = new TextBlock();
                textBlock1.Text = "• " + item;
                textBlock1.HorizontalAlignment = HorizontalAlignment.Left;
                textBlock1.VerticalAlignment = VerticalAlignment.Top;
                textBlock1.Margin = new Thickness(25, 50, 0, 0);
                textBlock1.Width = 250;
                textBlock1.Height = 30;
                textBlock1.FontSize = 13;

                Border border2 = new Border();
                border2.Width = 80;
                border2.Height = 30;
                border2.CornerRadius = new CornerRadius(5);
                border2.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FC7753");
                border2.BorderThickness = new Thickness(0);
                border2.HorizontalAlignment = HorizontalAlignment.Right;
                border2.Margin = new Thickness(0, 0, 40, 0);
                border2.PreviewMouseDown += DeleteBucketListItem;

                TextBlock textBlock = new TextBlock();
                textBlock.Text = "Delete";
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.FontWeight = FontWeights.SemiBold;
                textBlock.Foreground = Brushes.White;

                border2.Child = textBlock;

                grid.Children.Add(border1);
                grid.Children.Add(label);
                grid.Children.Add(border2);
                grid.Children.Add(textBlock1);

                bucketListStackPanel.Children.Add(grid);
            }
        }
    }

    public class BucketListItems
    {
        public List<string> items { get; set; }
    }
}

/*
CreateBucketListItem method XAML equivalent:

<Grid Height="90" VerticalAlignment="Top" Margin="3">
    <Border BorderBrush="#5BC65D" BorderThickness="3" CornerRadius="5"/>
    <Label Content="Bucket list item 1" FontWeight="SemiBold" FontSize="15" VerticalAlignment="Top" HorizontalAlignment="Left" Margin="20,15,0,0" Width="150" Height="30"/>
    <TextBox Text="• Visit Japan" Width="250" Height="30" VerticalAlignment="Top" FontSize="13" HorizontalAlignment="Left" Margin="25,50,0,0" BorderThickness="0"/>

    <Border Width="80" Height="30" CornerRadius="5" Background="#5BC65D" BorderThickness="1" HorizontalAlignment="Right" Margin="0,0,40,0">
        <TextBlock Text="Save" VerticalAlignment="Center" HorizontalAlignment="Center" FontWeight="SemiBold" Foreground="White"/>
    </Border>
</Grid>
 */
