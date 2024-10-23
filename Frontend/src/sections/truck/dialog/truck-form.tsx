import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Button,
  TextField,
  Select,
  MenuItem,
  SelectChangeEvent,
} from '@mui/material';
import { useState, useEffect } from 'react';
import type { TruckFormState } from 'src/interfaces/truck';

type TruckFormDialogProps = {
  open: boolean;
  onClose: () => void;
  onSubmit: (truck: TruckFormState) => void;
  truckToEdit?: TruckFormState | null;
};

type TruckDefinitions = {
  truckModels: { value: number; name: string; description: string }[];
  plantLocations: { value: number; name: string; description: string }[];
};

export function TruckFormDialog({
  open,
  onClose,
  onSubmit,
  truckToEdit = null,
}: TruckFormDialogProps) {
  const [truck, setTruck] = useState<TruckFormState>({
    id: '',
    model: 0,
    manufacturingYear: 0,
    chassisCode: '',
    color: '',
    plantName: 0,
  });

  const [definitions, setDefinitions] = useState<TruckDefinitions | null>(null);
  const [errors, setErrors] = useState<{
    manufacturingYear?: string;
    chassisCode?: string;
    model?: string;
    color?: string;
    plantName?: string;
  }>({});

  useEffect(() => {
    const fetchDefinitions = async () => {
      try {
        const response = await fetch(`${import.meta.env.VITE_API}/api/trucks/definitions`);
        const data: TruckDefinitions = await response.json();
        setDefinitions(data);
      } catch (error) {
        console.error('Erro ao buscar definições de caminhão:', error);
      }
    };

    fetchDefinitions();
  }, []);

  useEffect(() => {
    if (truckToEdit) {
      setTruck(truckToEdit);
    } else {
      setTruck({
        id: '',
        model: 0,
        manufacturingYear: 0,
        chassisCode: '',
        color: '',
        plantName: 0,
      });
    }
  }, [truckToEdit]);

  const validateManufacturingYear = (year: number) => {
    const currentYear = new Date().getFullYear();
    if (year < 1900) {
      setErrors((prev) => ({
        ...prev,
        manufacturingYear: 'Por favor, insira um ano posterior a 1900',
      }));
      return false;
    }
    if (year > currentYear) {
      setErrors((prev) => ({
        ...prev,
        manufacturingYear: 'Por favor, insira um ano anterior ou igual ao atual.',
      }));
      return false;
    }

    setErrors((prev) => ({ ...prev, manufacturingYear: undefined }));
    return true;
  };

  const validateRequiredFields = () => {
    const newErrors: any = {};
    if (!truck.model) newErrors.model = 'O modelo é obrigatório.';
    if (!truck.color) newErrors.color = 'A cor é obrigatória.';
    if (!truck.plantName) newErrors.plantName = 'O nome da planta é obrigatório.';
    if (!truck.manufacturingYear)
      newErrors.manufacturingYear = 'O ano de fabricação é obrigatório.';
    if (!truck.chassisCode) newErrors.chassisCode = 'O código do chassi é obrigatório.';
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;

    if (name === 'manufacturingYear') {
      const parsedValue = value ? parseInt(value, 10) : 0;
      setTruck((prev) => ({ ...prev, manufacturingYear: parsedValue }));

      validateManufacturingYear(parsedValue);
    } else {
      setTruck((prev) => ({ ...prev, [name!]: value }));
    }
  };

  const handleSelectChange = (e: SelectChangeEvent<number>) => {
    const { name, value } = e.target;
    setTruck((prev) => ({ ...prev, [name!]: value }));
  };

  const handleSubmit = () => {
    if (validateRequiredFields() && !errors.chassisCode && !errors.manufacturingYear) {
      setTruck({
        id: '',
        model: 0,
        manufacturingYear: 0,
        chassisCode: '',
        color: '',
        plantName: 0,
      });
      onSubmit(truck);
    }
  };

  return (
    <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
      <DialogTitle>{truckToEdit ? 'Editar Caminhão' : 'Novo Caminhão'}</DialogTitle>
      <DialogContent>
        {definitions ? (
          <>
            <Select
              fullWidth
              name="model"
              value={truck.model}
              onChange={handleSelectChange}
              displayEmpty
              margin="dense"
              error={Boolean(errors.model)}
            >
              <MenuItem value={0} disabled>
                Selecione um modelo
              </MenuItem>
              {definitions.truckModels.map((model) => (
                <MenuItem key={model.value} value={model.value}>
                  {model.name} - {model.description}
                </MenuItem>
              ))}
            </Select>
            {errors.model && <small style={{ color: 'red', marginLeft: 13 }}>{errors.model}</small>}

            <TextField
              fullWidth
              label="Ano de Fabricação"
              name="manufacturingYear"
              type="year"
              value={truck.manufacturingYear}
              onChange={handleChange}
              error={Boolean(errors.manufacturingYear)}
              helperText={errors.manufacturingYear}
              margin="normal"
            />

            <TextField
              fullWidth
              label="Código do Chassi"
              name="chassisCode"
              value={truck.chassisCode}
              onChange={handleChange}
              error={Boolean(errors.chassisCode)}
              helperText={errors.chassisCode}
              margin="normal"
            />

            <TextField
              fullWidth
              label="Cor"
              name="color"
              value={truck.color}
              onChange={handleChange}
              error={Boolean(errors.color)}
              helperText={errors.color}
              margin="normal"
            />

            <Select
              fullWidth
              name="plantName" // Alterado para corresponder ao estado
              value={truck.plantName}
              onChange={handleSelectChange}
              displayEmpty
              margin="dense"
              error={Boolean(errors.plantName)}
            >
              <MenuItem value={0} disabled>
                Selecione uma planta
              </MenuItem>
              {definitions.plantLocations.map((plant) => (
                <MenuItem key={plant.value} value={plant.value}>
                  {plant.name} - {plant.description}
                </MenuItem>
              ))}
            </Select>
            {errors.plantName && (
              <small style={{ color: 'red', marginLeft: 13 }}>{errors.plantName}</small>
            )}
          </>
        ) : (
          <p>Carregando definições...</p>
        )}
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} color="inherit">
          Cancelar
        </Button>
        <Button onClick={handleSubmit} variant="contained">
          {truckToEdit ? 'Salvar Alterações' : 'Criar Caminhão'}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
