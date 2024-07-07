float4 get_scaling(const float time, const float2 shadow_amplitude, const float2 shadow_speed)
{
    return float4(
        1 + sin(time * shadow_speed.x) * shadow_amplitude.x,
        1 + cos(time * shadow_speed.y) * shadow_amplitude.y,
        1,
        1
    );
}
