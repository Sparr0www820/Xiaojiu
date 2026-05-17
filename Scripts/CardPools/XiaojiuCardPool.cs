using Godot;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;

namespace Xiaojiu.Scripts.CardPools;

public class XiaojiuCardPool : TypeListCardPoolModel
{
    public override string Title => "xiaojiu";
    public override string EnergyColorName => "xiaojiu";
    public override string? TextEnergyIconPath => "res://Xiaojiu/images/energy_test.png";
    public override string? BigEnergyIconPath => "res://Xiaojiu/images/energy_test_big.png";
    public override Color DeckEntryCardColor => new(0.5f, 0.5f, 1f);
    public override Color EnergyOutlineColor => new(0.5f, 0.5f, 1f);
    private static readonly Material? _poolFrameMaterial = MaterialUtils.CreateRgbShaderMaterial(0.5f, 0.5f, 1f);
    public override Material? PoolFrameMaterial => _poolFrameMaterial;
    public override bool IsColorless => false;
}
