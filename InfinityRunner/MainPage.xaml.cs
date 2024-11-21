namespace InfinityRunner;

public partial class MainPage : ContentPage
{
	bool estaMorto=false;
	bool estaPulando=false;
	bool estaNoChao=true;
	bool estaNoAr=false;
	const int tempoEntreFrames=25;
	const int forcaGravidade=6;
	const int ForcaPulo=8;
	const int maxTempoPulando=6;
	const int maxTempoNoAr=4;
	int velocidade1=0;
	int velocidade2=0;
	int velocidade3=0;
	int velocidade=0;
	int largurJanela=0;
	int alturaJanela=0;
	int tempoPulando=0;
	int tempoNoAr=0;
	
	Player player;
	Inimigos inimigos;

	public MainPage()
	{
		InitializeComponent();
		player = new Player(imgCarro);
		player.Run();
	}

    protected override void OnSizeAllocated(double w, double h)
    {
        base.OnSizeAllocated(w, h);
		CorrigeTamanhoCenario(w,h);
		CalculaVelocidade(w);

		inimigos=new Inimigos(-w);
		inimigos.Add(new Inimigo(obstaculo1));
		inimigos.Add(new Inimigo(obstaculo2));
		inimigos.Add(new Inimigo(obstaculo3));
		inimigos.Add(new Inimigo(obstaculo4));
    }
	
	void OnGridTapped (object o, TappedEventArgs a)
	{
		if (estaNoChao)
			estaPulando=true;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		Desenha();
	}
	
	void CalculaVelocidade(double w)
	{
		velocidade1 = (int)(w*0.001);
		velocidade2 = (int)(w*0.004);
		velocidade3 = (int)(w*0.008);
		velocidade = (int)(w * 0.01);
	}

	void CorrigeTamanhoCenario(double w, double h)
	{
		foreach(var a in layerFundo1.Children)
			(a as Image ).WidthRequest = w;
		foreach(var a in layerFundo2.Children)
			(a as Image ).WidthRequest = w;
		foreach(var a in layerFundo3.Children)
			(a as Image ).WidthRequest = w;
		foreach( var a in layerAsfalto.Children)
			(a as Image ).WidthRequest = w;

		layerFundo1.WidthRequest=w*1.5;
		layerFundo2.WidthRequest=w*1.5;
		layerFundo3.WidthRequest=w*1.5;
		layerAsfalto.WidthRequest=w*1.5;
	}

	void GerenciaCenarios()
	{
		MoveCenario();
		GerenciaCenario(layerFundo1);
		GerenciaCenario(layerFundo2);
		GerenciaCenario(layerFundo3);
		GerenciaCenario(layerAsfalto);
	}

	void MoveCenario()
	{
		layerFundo1.TranslationX -= velocidade1;
		layerFundo2.TranslationX -= velocidade2;
		layerFundo3.TranslationX -= velocidade3;
		layerAsfalto.TranslationX -= velocidade;
	}
	
	void GerenciaCenario(HorizontalStackLayout hsl)
	{
		var view = (hsl.Children.First() as Image);
		if(view.WidthRequest + hsl.TranslationX < 0)
		{
			hsl.Children.Remove(view);
			hsl.Children.Add(view);
			hsl.TranslationX = view.TranslationX;
		}
	}

	void AplicaGravidade()
	{
		if (player.GetY()<0)
		player.MoveY(forcaGravidade);
		else if (player.GetY()>=0)
		{
			player.SetY(0);
			estaNoChao=true;
		}
	}
	void AplicaPulo()
	{
		estaNoChao=false;
		if(estaPulando && tempoPulando >=maxTempoPulando)
		{
			estaPulando=false;
			estaNoAr=true;
			tempoNoAr=0;
		}
		else if (estaNoAr && tempoNoAr >=maxTempoNoAr)
		{
			estaPulando=false;
			estaNoAr=false;
			tempoPulando=0;
			tempoNoAr=0;
		}
		else if (estaPulando && tempoPulando < maxTempoPulando)
		{
			player.MoveY (-ForcaPulo);
			tempoPulando ++;
		}
		else if (estaNoAr)
		tempoNoAr ++;
	}

	async Task Desenha()
	{
		while(!estaMorto)
		{
			GerenciaCenarios();
			if(inimigos != null)
				inimigos.Desenha(velocidade);
			if (!estaPulando && !estaNoAr)
			{
				AplicaGravidade();
				player.Desenha();
			}
			else 
				AplicaPulo();

			await Task.Delay(tempoEntreFrames);
		}
	}
}