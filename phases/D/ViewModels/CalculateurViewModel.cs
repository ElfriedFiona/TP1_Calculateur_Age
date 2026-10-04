namespace CalculateurAge.ViewModels;

public class CalculateurViewModel : BaseViewModel
{
    private string _nom = "";
    private DateTime _dateNaissance = DateTime.Today.AddYears(-20);
    private string _resultat = "";
    private bool _resultatVisible;
    private string _message = "";
    private string _prochainAnniversaire = "";

    public string Nom
    {
        get => _nom;
        set { if (SetField(ref _nom, value)) CalculerCommand.Rafraichir(); }
    }

    public DateTime DateNaissance
    {
        get => _dateNaissance;
        set
        {
            if (SetField(ref _dateNaissance, value))
            {
                OnPropertyChanged(nameof(DateFuture));   // fonctionnalité 3
                CalculerCommand.Rafraichir();
            }
        }
    }

    public string Resultat
    {
        get => _resultat;
        set => SetField(ref _resultat, value);
    }

    public bool ResultatVisible
    {
        get => _resultatVisible;
        set => SetField(ref _resultatVisible, value);
    }

    // Fonctionnalité 1 : message « Majeur » / « Mineur ».
    public string Message
    {
        get => _message;
        set => SetField(ref _message, value);
    }

    // Fonctionnalité 4 : jours restants avant le prochain anniversaire.
    public string ProchainAnniversaire
    {
        get => _prochainAnniversaire;
        set => SetField(ref _prochainAnniversaire, value);
    }

    // Fonctionnalité 3 : refus d'une date future.
    public bool DateFuture => DateNaissance.Date > DateTime.Today;

    public RelayCommand CalculerCommand { get; }
    public RelayCommand EffacerCommand { get; }   // Fonctionnalité 2

    public CalculateurViewModel()
    {
        CalculerCommand = new RelayCommand(
            Calculer,
            () => !string.IsNullOrWhiteSpace(Nom) && !DateFuture);
        EffacerCommand = new RelayCommand(Effacer);
    }

    private void Calculer()
    {
        int age = DateTime.Today.Year - DateNaissance.Year;
        if (DateNaissance.Date > DateTime.Today.AddYears(-age)) age--;

        Resultat = $"{Nom}, vous avez {age} ans";
        Message = age >= 18 ? "Majeur" : "Mineur";

        DateTime aujourdhui = DateTime.Today;
        DateTime prochain = AnniversaireEn(aujourdhui.Year);
        if (prochain < aujourdhui) prochain = AnniversaireEn(aujourdhui.Year + 1);
        int jours = (prochain - aujourdhui).Days;
        ProchainAnniversaire = jours == 0
            ? "Joyeux anniversaire !"
            : $"Prochain anniversaire dans {jours} jour(s)";

        ResultatVisible = true;
    }

    // Gère le 29 février les années non bissextiles.
    private DateTime AnniversaireEn(int annee)
    {
        int jour = Math.Min(DateNaissance.Day, DateTime.DaysInMonth(annee, DateNaissance.Month));
        return new DateTime(annee, DateNaissance.Month, jour);
    }

    private void Effacer()
    {
        Nom = "";
        DateNaissance = DateTime.Today.AddYears(-20);
        Resultat = "";
        Message = "";
        ProchainAnniversaire = "";
        ResultatVisible = false;
    }
}
