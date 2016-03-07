#region Usings 

using Storm.Mvvm;

#endregion

namespace IndiaRose.Data.Model
{
    /// <summary>
    /// Classe décrivant un Indiagram
    /// </summary>
	public class Indiagram : NotifierBase
	{
		private int _id = -1;
		private string _imagePath;
		private bool _isEnabled = true;
		private Indiagram _parent;
		private int _position;
		private string _soundPath;
		private string _text;

        /// <summary>
        /// Id de l'Indiagram (utile uniquement pour la BD)
        /// Par défaut à -1
        /// </summary>
		public int Id
		{
			get { return _id; }
			set { SetProperty(ref _id, value); }
		}

        /// <summary>
        /// Position de l'Indiagram dans sa catégorie
        /// </summary>
		public int Position
		{
			get { return _position; }
			set { SetProperty(ref _position, value); }
		}

        /// <summary>
        /// Texte de l'Indiagram (sera possiblement lu par le TTS)
        /// </summary>
		public string Text
		{
			get { return _text; }
			set { SetProperty(ref _text, value); }
		}

        /// <summary>
        /// Chemin d'accès vers l'image de l'Indiagram
        /// </summary>
		public string ImagePath
		{
			get { return _imagePath; }
			set { SetProperty(ref _imagePath, value); }
		}

        /// <summary>
        /// Chemin d'accès vers le son de l'Indiagram
        /// </summary>
		public string SoundPath
		{
			get { return _soundPath; }
		    set
		    {
		        SetProperty(ref _soundPath, value);
		        RaisePropertyChanged("HasCustomSound");
		    }
		}

        /// <summary>
        /// Parent de l'Indiagram (Indiagram qui le contient)
        /// </summary>
		public Indiagram Parent
		{
			get { return _parent; }
			set { SetProperty(ref _parent, value); }
		}

        /// <summary>
        /// Vrai si l'Indiagram est activé
        /// Si l'Indiagram est désactivé il ne sera pas affiché dans la partie utilisateur
        /// </summary>
		public bool IsEnabled
		{
			get { return _isEnabled; }
			set { SetProperty(ref _isEnabled, value); }
		}
        /// <summary>
        /// Vrai si l'Indiagram est une catégorie
        /// </summary>
		public virtual bool IsCategory
		{
			get { return false; }
		}

        /// <summary>
        /// Vrai si l'Indiagram contient d'autres Indiagrams
        /// </summary>
		public virtual bool HasChildren
		{
			get { return false; }
		}

        /// <summary>
        /// Vrai si l'Indiagram a un son (le texte ne devant pas être lu par le TTS)
        /// </summary>
		public bool HasCustomSound
		{
			get { return !string.IsNullOrWhiteSpace(SoundPath); }
		}

        /// <summary>
        /// Copie les paramètres d'un autre Indiagram
        /// Copie ou non l'ID
        /// </summary>
        /// <param name="other">L'Indiagram qui doit être copié</param>
        /// <param name="excludeId">Vrai si on veut copier l'ID (par défaut à faux)</param>
		public virtual void CopyFrom(Indiagram other, bool excludeId = false)
		{
			Text = other.Text;
			ImagePath = other.ImagePath;
			SoundPath = other.SoundPath;
			IsEnabled = other.IsEnabled;
			Position = other.Position;
			Parent = other.Parent;
			if (!excludeId)
			{
				Id = other.Id;
			}
		}

        /// <summary>
        /// Teste si 2 Indiagrams sont identiques (s'ils ont le même ID)
        /// Retourne vrai si les 2 paramètres sont null
        /// </summary>
        /// <returns>Le résultat de la comparaison des 2 Indiagrams</returns>
		public static bool AreSameIndiagram(Indiagram a, Indiagram b)
		{
			if (a == null && b == null)
			{
				return true;
			}

			if (a == null || b == null)
			{
				return false;
			}

			return a.Id == b.Id;
		}
	}
}