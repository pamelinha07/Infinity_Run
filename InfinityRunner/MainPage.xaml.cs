namespace InfinityRunner;

public partial class MainPage : ContentPage
{
	bool estaMorto=false;
	bool estaPulando=false;
	const int tempoEntreFrames=25;
	int velocidade1=0;
	int velocidade2=0;
	int velocidade3=0;
	int velocidade=0;
	int largurJanela=0;
	int alturaJanela=0;
	
	Player player;

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
    }

	protected override void OnAppearing()
	{
		base.OnAppearing();
		Desenha();
	}
	
	void CalculaVelocidade(double w)
	{
		velocidade1=(int)(w*0.001);
		velocidade2=(int)(w*0.004);
		velocidade3=(int)(w*0.008);
		velocidade = (int) (w * 0.01);
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
		if(view.WidthRequest+hsl.TranslationX<0)
		{
			hsl.Children.Remove(view);
			hsl.Children.Add(view);
			hsl.TranslationX = view.TranslationX;
		}
	}

	async Task Desenha()
	{
		while(!estaMorto)
		{
			GerenciaCenarios();
			player.Desenha();
			await Task.Delay(tempoEntreFrames);
		}
	}
}