using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace MauiSample;

public partial class Page2 : ContentPage
{
	ObservableCollection<GPUModel> gpuList = new ObservableCollection<GPUModel>();
	public Page2()
	{
		InitializeComponent();
		NavigationPage.SetHasNavigationBar(this, false);
		NavigationPage.SetHasBackButton(this, true);

		gpuList.Add(new GPUModel(){ id=0, brand = "NVIDIA", model="RTX 5090", image = "zotac_rtx_5090.jpg" });
		gpuList.Add(new GPUModel(){ id=1, brand = "NVIDIA", model="GTX 1080 Ti", image = "gtx_1080ti.jpg" });
		gpuList.Add(new GPUModel(){ id=2, brand = "NVIDIA", model="GTX 1050 Ti", image = "gtx_1050ti.jpg" });
		gpuList.Add(new GPUModel(){ id=3, brand = "AMD", model="RX 6700XT", image = "https://tpucdn.com/gpu-specs/images/c/3695-front.jpg" });
		gpuList.Add(new GPUModel(){ id=4, brand = "AMD", model="RX 580", image = "https://tpucdn.com/gpu-specs/images/c/2938-front.jpg" });
		gpuLV.ItemsSource = gpuList;
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		Navigation.PopModalAsync();
    }

    private void GpuLV_OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
	    GPUModel item = (e.SelectedItem) as GPUModel;
	    Console.WriteLine("Item selected: ",item);
	    
	    //((ListView)sender).SelectedItem = null; //remove the highlight
    }

    private void GpuLV_OnItemTapped(object? sender, ItemTappedEventArgs e)
    {
	    GPUModel item = (e.Item) as GPUModel;
	    Console.WriteLine("Item tapped: ",item);
	    gpuList.Remove(gpuList.Where(i => i.id == item.id).Single());
    }
}