using System.Collections.ObjectModel;

namespace Diskretka_7;

public partial class MainPage : ContentPage
{
    static int N = 12;

    int[] weights = new int[N];
    int[] nOut = new int[N];
    List<int> sortedIDs = new List<int>();


    ObservableCollection<(int, int)> dugi = new ObservableCollection<(int, int)>()
        {
            (0,1),
            (0,2),
            (1,2),
            (2,3),
            (2,4),
            (3,7),
            (4,5),
            (4,7),
            (6,8),
            (7,6),
            (8,9),
            (10,8),
            (11,9)
        };


    int[] weights12 = { 3, 7, 8, 2, 3, 5, 6, 4, 1, 3, 7, 10 };

    bool weightsSet = false;
    Color error = Color.FromRgb(255, 233, 236);
    Color valid = Color.FromRgb(236, 255, 233);


    public MainPage()
    {
        InitializeComponent();
        generateWeights();
        dugiList.ItemsSource = dugi;

        foreach ((int, int) duga in dugi)
        {
            nOut[duga.Item1]++;
        }

        for (int i = 0; i < N; i++)
        {
            resultGrid.AddRowDefinition(new RowDefinition() { Height = 50 });
            queueGrid.AddColumnDefinition(new ColumnDefinition() { Width = 50 });
        }
    }


    void setWeights(System.Object sender, System.EventArgs e)
    {
        sortedIDs.Clear();
        resultGrid.Clear();
        queueGrid.Clear();
        resultHeader.IsVisible = false;
        sortButton.IsVisible = false;
        resetButton.IsVisible = false;
        sortButton.IsEnabled = true;

        from.Text = String.Empty;
        to.Text = String.Empty;
        from.BackgroundColor = Color.FromArgb("#00FFFFFF");
        to.BackgroundColor = Color.FromArgb("#00FFFFFF");

        int count = 0;
        weightsSet = true;

        foreach (Entry entry in weightsEntries.Children)
        {
            try
            {
                int value = Int16.Parse(entry.Text);
                if (value <= 0 || entry.Text.Length == 0)
                {
                    entry.BackgroundColor = error;
                    entry.Focus();
                    weightsSet = false;
                }
                else
                {
                    weights[count++] = value;
                    entry.BackgroundColor = valid;
                }
            }
            catch (System.FormatException)
            {
                entry.BackgroundColor = error;
                entry.Focus();
                weightsSet = false;
            }
        }

        if (weightsSet)
        {
            resultHeader.IsVisible = true;
            sortButton.IsVisible = true;
            resetButton.IsVisible = true;
            queueFrame.IsVisible = true;

            resultGrid.AddColumnDefinition(new ColumnDefinition() { Width = 50 });
            resultGrid.AddColumnDefinition(new ColumnDefinition() { Width = 50 });
            resultGrid.AddColumnDefinition(new ColumnDefinition() { Width = 50 });
            
            for (int i = 0; i < N; i++)
            {
                resultGrid.Add(new Frame { BackgroundColor = valid, HeightRequest = 40, Padding = 0, Margin = 0, Content = new Label { Margin = 0, Padding = 0, Text = (i).ToString(), HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center } }, 0, i);
                resultGrid.Add(new Frame { BackgroundColor = valid, HeightRequest = 40, Padding = 0, Margin = 0, Content = new Label { Margin = 0, Padding = 0, Text = weights[i].ToString(), HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center } }, 1, i);
                resultGrid.Add(new Frame { BackgroundColor = valid, HeightRequest = 40, Padding = 0, Margin = 0, Content = new Label { Margin = 0, Padding = 0, Text = nOut[i].ToString(), HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center } }, 2, i);
                nOut[i] = 0;
            }

            foreach((int, int) duga in dugi)
            {
                nOut[duga.Item1]++;
            }
        }
        else
        {
            DisplayAlert("Ошибка", "Неверные значения весов", "Ok");
        }
    }


    void onSortClicked(System.Object sender, System.EventArgs e)
    {
        edgeAddButton.IsEnabled = false;

        int min = int.MaxValue;
        int id = 0;

        for (int i = 0; i < N; i++)
        {
            if (!sortedIDs.Contains(i) && nOut[i] == 0 && weights[i] < min)
            {
                id = i;
                min = weights[i];
            }
        }

        sortedIDs.Add(id);

        queueGrid.Add(new Frame { HeightRequest = 40, Padding = 0, Margin = 0, Content = new Label { Margin = 0, Padding = 0, Text = (id).ToString(), HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center } }, N - sortedIDs.Count);

        foreach ((int, int) duga in dugi)
        {
            if (duga.Item2 == id)
            {
                nOut[duga.Item1]--;
            }
        }

        resultGrid.AddColumnDefinition(new ColumnDefinition() { Width = 50 });
        int number = sortedIDs.Count;
        for (int i = 0; i < N; i++)
        {
            Color backGroundColor = sortedIDs.Contains(i) ? error : Color.FromArgb("#00FFFFFF");
            resultGrid.Add(new Frame { BackgroundColor = backGroundColor, HeightRequest = 40, Padding = 0, Margin = 0, Content = new Label { Margin = 0, Padding = 0, Text = nOut[i].ToString(), HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center } }, 2 + number, i);
        }

        if (sortedIDs.Count == N)
        {
            DisplayAlert("Сообщение", "Сортировка успешна завершена", "Хорошо");
            sortButton.IsEnabled = false;
        }
    }


    void generateWeights()
    {
        for (int i = 0; i < N; i++)
        {
            Label label = new Label { Text = (i).ToString() };
            Entry entry = new Entry { Placeholder = i.ToString(), MaxLength = 2, Text = N == 12 ? weights12[i].ToString() : String.Empty };
            entry.Focused += onFocus;

            weightsLabels.Children.Add(label);
            weightsEntries.Children.Add(entry);
        }
    }


    void outputReset(System.Object sender, System.EventArgs e)
    {
        resultGrid.Clear();

        resultHeader.IsVisible = false;
        sortButton.IsVisible = false;
        resetButton.IsVisible = false;
        queueFrame.IsVisible = false;

        sortButton.IsEnabled = true;
        edgeAddButton.IsEnabled = true;

        foreach (Entry entry in weightsEntries.Children)
        {
            entry.BackgroundColor = Color.FromArgb("#00FFFFFF");
        }
    }


    static void onFocus(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        var entry = sender as Entry;

        entry.CursorPosition = 0;
        entry.SelectionLength = entry.Text == null ? 0 : entry.Text.Length;
    }


    void addEdge(System.Object sender, System.EventArgs e)
    {
        bool inputValid = true;

        int otkuda = 0, kuda = 0;

        try
        {
            otkuda = Int32.Parse(from.Text);
            if (otkuda >= N || otkuda < 0)
            {
                from.BackgroundColor = error;
                inputValid = false;
                from.Focus();
                DisplayAlert("Ошибка", "Неверное значениe вершины начала", "Ok");
            }
        }
        catch (System.FormatException)
        {
            inputValid = false;
            from.BackgroundColor = error;
            from.Focus();
        }

        try
        {
            kuda = Int32.Parse(to.Text);
            if (kuda >= N || kuda < 0)
            {
                to.BackgroundColor = error;
                inputValid = false;
                to.Focus();
                DisplayAlert("Ошибка", "Неверное значениe вершины конца", "Ok");
            }
        }
        catch (System.FormatException)
        {
            inputValid = false;
            to.BackgroundColor = error;
            to.Focus();
        }


        if (inputValid)
        {
            foreach ((int, int) duga in dugi)
            {
                if (duga.Item1 == otkuda && duga.Item2 == kuda)
                {
                    from.BackgroundColor = to.BackgroundColor = error;
                    DisplayAlert("Ошибка", "Дуга уже в списке", "Ок");
                    return;
                }
                if (duga.Item2 == otkuda && duga.Item1 == kuda)
                {
                    from.BackgroundColor = to.BackgroundColor = error;
                    DisplayAlert("Ошибка", "Вершины уже соединены обратной дугой", "Ок");
                    return;
                }
            }

            if (kuda == otkuda)
            {
                from.BackgroundColor = to.BackgroundColor = error;
                DisplayAlert("Ошибка", "Дуга соединяет один и тот же элемент", "Ок");
            }
            else
            {
                dugi.Add((otkuda, kuda));
                dugiList.ItemsSource = dugi;
                from.Text = String.Empty;
                to.Text = String.Empty;
                from.BackgroundColor = Color.FromArgb("#00FFFFFF");
                to.BackgroundColor = Color.FromArgb("#00FFFFFF");
                DisplayAlert("Успех", $"Дуга из {otkuda} в {kuda} успешно добавлена", "Ок");
            }
        }
        else
        {
            DisplayAlert("Ошибка", "Нечисловые значения вершин", "Ok");
        }
    }
}