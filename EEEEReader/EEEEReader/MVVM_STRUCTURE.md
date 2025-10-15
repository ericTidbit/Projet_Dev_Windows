# Structure MVVM du projet EEEEReader

## Architecture

Ce projet utilise le pattern **MVVM (Model-View-ViewModel)** pour s�parer la logique m�tier de l'interface utilisateur.

## Structure des dossiers

```
EEEEReader/
??? Models/                          # Mod�les de donn�es
?   ??? Message.cs                   # Exemple: classe repr�sentant un message
?
??? Views/                           # Vues (interfaces utilisateur XAML)
?   ??? (� cr�er selon vos besoins)
?
??? ViewModels/                      # ViewModels (logique de pr�sentation)
?   ??? ViewModelBase.cs            # Classe de base pour tous les ViewModels
?   ??? RelayCommand.cs             # Impl�mentation de ICommand
?   ??? MainViewModel.cs            # ViewModel de la fen�tre principale
?   ?
?   ??? Pages/                       # ViewModels sp�cifiques aux pages
?       ??? MessagesPageViewModel.cs # ViewModel pour la gestion de messages
?
??? MainWindow.xaml                  # Fen�tre principale
??? MainWindow.xaml.cs              # Code-behind de la fen�tre principale
```

## Composants MVVM

### 1. Models (Mod�les)
Les mod�les repr�sentent les donn�es de l'application.
- **Message.cs** : Exemple de mod�le avec propri�t�s (Id, Contenu, Auteur, DateCreation)

### 2. ViewModels
Les ViewModels contiennent la logique de pr�sentation et exposent les donn�es aux vues.

#### Classes utilitaires :
- **ViewModelBase** : Classe abstraite de base
  - Impl�mente `INotifyPropertyChanged`
  - Fournit `OnPropertyChanged()` pour notifier les changements de propri�t�
  - Fournit `SetProperty()` pour simplifier la mise � jour des propri�t�s
  
- **RelayCommand** : Impl�mentation de `ICommand`
  - Permet de lier des actions aux boutons
  - Supporte `CanExecute` pour activer/d�sactiver les commandes
  - M�thode `LeverChangementPeutExecuter()` pour forcer la r��valuation

#### ViewModels impl�ment�s :
- **MainViewModel** : ViewModel principal de la fen�tre
  - H�rite de `ViewModelBase`
  - Propri�t�s : 
    - `TexteEntree` : Texte saisi par l'utilisateur
    - `TexteAffiche` : Texte affich� � l'�cran
  - Commande : 
    - `CommandeEnvoyer` : Copie le texte d'entr�e vers le texte affich�
  
- **MessagesPageViewModel** : ViewModel pour la gestion de messages (exemple)
  - H�rite de `ViewModelBase`
  - Propri�t�s : 
    - `Messages` : ObservableCollection de messages
    - `MessageSelectionne` : Message actuellement s�lectionn�
    - `NouveauMessageTexte` : Texte du nouveau message � ajouter
  - Commandes : 
    - `CommandeAjouterMessage` : Ajoute un nouveau message
    - `CommandeSupprimerMessage` : Supprime le message s�lectionn�

### 3. MainWindow (Fen�tre principale)
La fen�tre principale utilise le MainViewModel :
- **TextBox** li� � `TexteEntree` (bidirectionnel)
- **Button** "Envoyer" li� � `CommandeEnvoyer`
- **TextBlock** li� � `TexteAffiche` (lecture seule)

## Principes MVVM

### 1. Binding (Liaison de donn�es)
Les propri�t�s du ViewModel sont li�es aux contr�les de la vue via `{Binding}` :
```xaml
<!-- Liaison bidirectionnelle pour le TextBox -->
<TextBox Text="{Binding TexteEntree, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"/>

<!-- Liaison unidirectionnelle pour le TextBlock -->
<TextBlock Text="{Binding TexteAffiche}" />
```

### 2. Commandes
Les actions utilisateur sont g�r�es par des commandes au lieu d'�v�nements :
```xaml
<Button Command="{Binding CommandeEnvoyer}" Content="Envoyer" />
```

### 3. Notifications de changement
Quand une propri�t� change, le ViewModel notifie automatiquement la vue via `INotifyPropertyChanged` :
```csharp
public string TexteAffiche
{
    get => _texteAffiche;
    set => SetProperty(ref _texteAffiche, value); // Notifie automatiquement
}
```

La m�thode `SetProperty()` de `ViewModelBase` :
- Compare l'ancienne et la nouvelle valeur
- Met � jour le champ si diff�rent
- Appelle `OnPropertyChanged()` pour notifier la vue
- Retourne `true` si la valeur a chang�

## Avantages de cette structure

1. **S�paration des pr�occupations** : Interface et logique sont s�par�es
2. **Testabilit�** : Les ViewModels peuvent �tre test�s sans interface
3. **Maintenabilit�** : Code organis� et facile � modifier
4. **R�utilisabilit�** : ViewModelBase et RelayCommand sont r�utilisables
5. **Data Binding** : Synchronisation automatique entre vue et donn�es
6. **Pas de code-behind** : Toute la logique est dans les ViewModels

## Guide d'utilisation

### Cr�er un nouveau ViewModel

```csharp
using System.Windows.Input;

namespace EEEEReader.ViewModels
{
    public class MonViewModel : ViewModelBase
    {
        private string _maPropriete = string.Empty;
        
        public string MaPropriete
        {
            get => _maPropriete;
            set => SetProperty(ref _maPropriete, value);
        }
        
        public ICommand MaCommande { get; }
        
        public MonViewModel()
        {
            MaCommande = new RelayCommand(_ => ExecuterAction());
        }
        
        private void ExecuterAction()
        {
            // Votre logique ici
            MaPropriete = "Nouvelle valeur";
        }
    }
}
```

### Cr�er une nouvelle Vue (Page WinUI 3)

1. **Cr�er le fichier XAML** (ex: `MaPage.xaml`) :
```xaml
<Page
    x:Class="EEEEReader.Views.MaPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:vm="using:EEEEReader.ViewModels">

    <Grid>
        <Grid.DataContext>
            <vm:MonViewModel />
        </Grid.DataContext>
        
        <StackPanel>
            <TextBox Text="{Binding MaPropriete, Mode=TwoWay}" />
            <Button Command="{Binding MaCommande}" Content="Action" />
        </StackPanel>
    </Grid>
</Page>
```

2. **Cr�er le code-behind** (ex: `MaPage.xaml.cs`) :
```csharp
using Microsoft.UI.Xaml.Controls;

namespace EEEEReader.Views
{
    public sealed partial class MaPage : Page
    {
        public MaPage()
        {
            this.InitializeComponent();
        }
    }
}
```

### Utiliser ObservableCollection pour les listes

Pour afficher une liste dynamique qui se met � jour automatiquement :

```csharp
using System.Collections.ObjectModel;

public class MonViewModel : ViewModelBase
{
    public ObservableCollection<Message> Messages { get; }
    
    public MonViewModel()
    {
        Messages = new ObservableCollection<Message>();
    }
    
    private void AjouterMessage()
    {
        Messages.Add(new Message { Contenu = "Nouveau message" });
    }
}
```

```xaml
<ListView ItemsSource="{Binding Messages}">
    <ListView.ItemTemplate>
        <DataTemplate>
            <TextBlock Text="{Binding Contenu}" />
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

## Technologies utilis�es
- **.NET 8** : Framework de d�veloppement
- **WinUI 3** : Interface utilisateur moderne pour Windows
- **C# 12** : Langage de programmation
- **MVVM Pattern** : Architecture de s�paration des pr�occupations

## Exemple fonctionnel : MainWindow

Le projet contient un exemple fonctionnel dans `MainWindow.xaml` :
- Un `TextBox` pour saisir du texte
- Un `Button` pour envoyer le texte
- Un `TextBlock` pour afficher le texte

L'utilisateur tape du texte, clique sur "Envoyer", et le texte appara�t dans le TextBlock.
Tout est g�r� via le pattern MVVM sans code dans le code-behind !

## Prochaines �tapes

Pour �tendre l'application :
1. Cr�er de nouveaux Models dans `Models/`
2. Cr�er des ViewModels correspondants dans `ViewModels/` ou `ViewModels/Pages/`
3. Cr�er des Pages XAML dans `Views/` avec leurs ViewModels
4. Utiliser `Frame.Navigate()` pour naviguer entre les pages si n�cessaire
