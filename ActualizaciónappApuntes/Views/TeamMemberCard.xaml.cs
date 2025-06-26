using ActualizaciónappApuntes.Models;

namespace ActualizaciónappApuntes.Views;

public partial class TeamMemberCard : ContentView
{
    public static readonly BindableProperty MemberProperty =
        BindableProperty.Create(nameof(Member), typeof(Member), typeof(TeamMemberCard), null);

    public Member Member
    {
        get => (Member)GetValue(MemberProperty);
        set => SetValue(MemberProperty, value);
    }

    public TeamMemberCard()
    {
        InitializeComponent();
    }

    protected override void OnPropertyChanged(string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == nameof(Member))
        {
            BindingContext = Member;
        }
    }
}