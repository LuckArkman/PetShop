using System;
using System.Collections.Generic;

namespace DTOs;

public class HistoryFrequenciaCardiaca
{
    public string id { get; set; } = Guid.NewGuid().ToString();
    public string AnimalId { get; set; }
    public ICollection<FrequenciaCardiaca> _frequenciaCardiaca { get; set; }

    public HistoryFrequenciaCardiaca(string id, string animalId, FrequenciaCardiaca frequenciaCardiaca)
    {
        this.id = id;
        AnimalId = animalId;
        _frequenciaCardiaca = new List<FrequenciaCardiaca>() { frequenciaCardiaca };
    }
}