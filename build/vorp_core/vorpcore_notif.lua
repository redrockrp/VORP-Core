RegisterNetEvent('vorp:NotifyLeft')
AddEventHandler('vorp:NotifyLeft', function(firsttext, secondtext, dict, icon, duration)
    if not HasStreamedTextureDictLoaded(tostring(dict)) then
        RequestStreamedTextureDict(tostring(dict), true) 
            while not HasStreamedTextureDictLoaded(tostring(dict)) do
                Wait(0)
            end
    end
    if icon ~= nil then
        exports.vorp_core.DisplayLeftNotification(0, tostring(firsttext), tostring(secondtext), tostring(dict), tostring(icon), tonumber(duration))
    else
        local icon = "tick"
        exports.vorp_core.DisplayLeftNotification(0, tostring(firsttext), tostring(secondtext), tostring(dict), tostring(icon), tonumber(duration))
    end
end)

RegisterNetEvent('vorp:Tip')
AddEventHandler('vorp:Tip', function(text, duration)
exports.vorp_core.DisplayTip(0, tostring(text), tonumber(duration))
end)

RegisterNetEvent('vorp:NotifyTop')
AddEventHandler('vorp:NotifyTop', function(text, duration)
exports.vorp_core.DisplayTopCenterNotification(0, tostring(text), tostring(text), tonumber(duration))
end) 