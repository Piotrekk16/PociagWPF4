using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication20
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Pociag pociag = new Pociag(); //prywatne pole na obiekt "pociąg"

        public MainWindow()
        {
            InitializeComponent();
            Ukryj();
        }

        public void Ukryj() //metoda ukrywająca gridy
        {
            PanelLokomotywa.Visibility = Visibility.Hidden;
            PanelWagon.Visibility = Visibility.Hidden;
        }

        private void DodajLokomotywe_Click(object sender, RoutedEventArgs e) //odkrycie grida z dodawaniem lokomotyw
        {
            PanelLokomotywa.Visibility = Visibility.Visible;
            PanelWagon.Visibility = Visibility.Hidden;
        }

        private void DodajL_Click(object sender, RoutedEventArgs e) //dodanie lokomotywy do pociągu
        {
            try
            {
                int masaLok; //zmienna pomocnicza
                masaLok = Convert.ToInt32(MasaL.Text); //konwersja tekstu na liczbę typu int
                if (masaLok <= 0)
                    throw new ArgumentOutOfRangeException(); //sprawdzenie czy masa będzie dodatnia
                pociag.DodajLokomotywe(ModelL.Text, masaLok);
                SkladPociagu.Text = pociag.Informacje();
            }
            catch
            {
                MessageBox.Show("Sprawdź poprawność wprowadzonych danych!");
            }

        }

        private void Ukryj_Click(object sender, RoutedEventArgs e) //ukrycie gridów
        {
            Ukryj();
        }

        private void DodajWagon_Click(object sender, RoutedEventArgs e) //odkrycie grida z dodawaniem wagonów
        {
            PanelLokomotywa.Visibility = Visibility.Hidden;
            PanelWagon.Visibility = Visibility.Visible;
        }

        private void DodajW_Click(object sender, RoutedEventArgs e) //dodanie wagonu do pociągu
        {
            try
            {
                int masaWag; //zmienna pomocnicza
                masaWag = Convert.ToInt32(MasaW.Text); //konwersja string ns int
                if (masaWag <= 0)
                    throw new ArgumentOutOfRangeException(); //sprawdzenie warunku aby masa była dodatnia
                if (Typ.SelectedIndex == 0)
                {
                    pociag.DodajOsobowy(ModelW.Text, masaWag, RodzajLadunek.Text);
                }
                else if (Typ.SelectedIndex == 1)
                {
                    pociag.DodajTowarowy(ModelW.Text, masaWag, RodzajLadunek.Text);
                }
                else //wyrzucenie wyjątku w sytuacji kiedy w liście rozwijanej nie jest nic wybrane
                {
                    throw new ArgumentNullException();
                }
                SkladPociagu.Text = pociag.Informacje();
            }
            catch
            {
                MessageBox.Show("Sprawdź poprawność wprowadzonych danych!");
            }
        }

        private void SprawdzPociag_Click(object sender, RoutedEventArgs e) //wyświetlenie okienka z informacją czy pociąg może jechać
        {
            if (pociag.MozeJechac())
            {
                MessageBox.Show("Pociąg może jechać!");
            }
            else
            {
                MessageBox.Show("Ups, coś jest nie tak! Pociąg nie może jechać!");
            }
        }
    }

    class Pociag : IUzupelnijSklad, IPoprawnyPociag //podpięcie interfejsów
    {
        //deklaracja pól w klasie
        private List<Lokomotywa> lokomotywy = new List<Lokomotywa>();
        private List<Wagon> wagony = new List<Wagon>();

        public bool MozeJechac() //metoda sprawdzająca czy pociag moze jechac
        {
            int masaL = 0; //zmienna pomocnicza na masę wszystkich lokomotyw
            for (int i = 0; i < lokomotywy.Count; i++)
            {
                masaL += lokomotywy[i].PobierzMase();
            }

            int masaW = 0; //zmienna pomocniczna na masę wszystkich wagonów
            foreach (var element in wagony)
            {
                masaW += element.PobierzMase();
            }

            //instrukcja na sprawdzenie warunków
            if (masaL >= masaW && lokomotywy.Count > 0) return true;
            else return false;
        }

        public void DodajLokomotywe(string model, int masa) //metoda dodająca lokomotywę do składu pociągu
        {
            lokomotywy.Add(new Lokomotywa(model, masa));
        }

        public void DodajOsobowy(string model, int masa, string rodzaj) //metoda dodająca wagon osobowy do składu pociągu
        {
            wagony.Add(new Osobowy(model, masa, rodzaj));
        }

        public void DodajTowarowy(string model, int masa, string ladunek) //metoda dodająca wagon towarowy do składu pociągu
        {
            wagony.Add(new Towarowy(model, masa, ladunek));
        }

        public string Informacje() //metoda zwracająca informacje o składzie pociągu
        {
            string info = "Skład pociągu:"; //zmienna pomocnicza
            for (int i = 0; i < lokomotywy.Count; i++)
            {
                info += Environment.NewLine + lokomotywy[i].Informacje();
            }
            foreach (var element in wagony)
            {
                info += Environment.NewLine + element.Informacje();
            }
            return info;

        }

    }

    interface IPoprawnyPociag
    {
        //deklaracja metody ktorej zadaniem jest sprawdzenie czy pociag moze jechac
        bool MozeJechac();
    }

    interface IUzupelnijSklad
    {
        //deklaracja metod związanych z uzupełnieniem składu pociągu
        void DodajOsobowy(string model, int masa, string rodzaj);
        void DodajTowarowy(string model, int masa, string ladunek);
        void DodajLokomotywe(string model, int masa);
    }

    class Lokomotywa
    {
        //deklaracja pól w klasie
        private string model;
        private int masa;

        public Lokomotywa(string model, int masa) //konstruktor parametryczny
        {
            this.model = model;
            this.masa = masa;
        }

        public int PobierzMase() //metoda zwracająca masę
        {
            return masa;
        }

        public string Informacje() //metoda zwracająca informacje o lokomotywie
        {
            return "Lokomotywa: model " + model + ", masa " + masa;
        }
    }

    abstract class Wagon
    {
        //deklaracja pól w klasie
        protected string model;
        protected int masa;

        abstract public string Informacje(); //deklaracja abstrakcyjnej metody dla zwracania informacji o wagonie

        public int PobierzMase() //metoda zwracająca masę wagonu
        {
            return masa;
        }

        public Wagon(string model, int masa) //konstruktor parametryczny
        {
            this.model = model;
            this.masa = masa;
        }
    }

    class Osobowy : Wagon //dziedziczenie
    {
        //deklaracja pola
        private string rodzaj;

        public Osobowy(string model, int masa, string rodzaj) //konstruktor parametryczny
            : base(model, masa)
        {
            this.rodzaj = rodzaj;
        }

        public override string Informacje() //metoda zwracająca informacje o wagonie osobowym
        {
            return "Wagon osobowy: model " + model + ", masa " + masa + ", rodzaj " + rodzaj;
        }
    }

    class Towarowy : Wagon //dziedziczenie
    {
        //deklaracja pola
        private string ladunek;

        public Towarowy(string model, int masa, string ladunek) //konstruktor parametryczny
            : base(model, masa)
        {
            this.ladunek = ladunek;
        }

        public override string Informacje() //metoda zwracająca informacje o wagonie towarowym
        {
            return "Wagon towarowy: model " + model + ", masa " + masa + ", ładunek " + ladunek;
        }
    }




}
