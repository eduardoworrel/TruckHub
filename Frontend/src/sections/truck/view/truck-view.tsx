import { useState, useEffect, useCallback } from 'react';


import Box from '@mui/material/Box';
import Card from '@mui/material/Card';
import Table from '@mui/material/Table';
import Button from '@mui/material/Button';
import TableBody from '@mui/material/TableBody';
import Typography from '@mui/material/Typography';
import TableContainer from '@mui/material/TableContainer';
import TablePagination from '@mui/material/TablePagination';

import type { TruckDefinitions, TruckFormState, TrucksResponse } from 'src/interfaces/truck';

import { DashboardContent } from 'src/layouts/dashboard';

import { Iconify } from 'src/components/iconify';
import { Scrollbar } from 'src/components/scrollbar';

import ConfirmDelete from '../dialog/confirm-delete';
import { SimpleDialog } from '../dialog/feedback';
import { TruckFormDialog } from '../dialog/truck-form';
import { TableNoData } from '../table-no-data';
import { TruckTableRow } from '../truck-table-row';
import { UserTableHead } from '../truck-table-head';
import { TableEmptyRows } from '../table-empty-rows';
import { UserTableToolbar } from '../truck-table-toolbar';
import { emptyRows, applyFilter, getComparator } from '../utils';

export function TruckView() {
  const table = useTable();
  const [success, setSuccess] = useState('');
  const [filterName, setFilterName] = useState('');
  const [trucks, setTrucks] = useState<TrucksResponse[]>([]);
  const [openDeleteDialog, setOpenDeleteDialog] = useState(false);
  const [openFormDialog, setOpenFormDialog] = useState(false);
  const [currentTruckToEdit, setCurrentTruckToEdit] = useState<TruckFormState | null>(null);
  const [truckDefinitions, setTruckDefinitions] = useState<TruckDefinitions | null>(null);

  const handleCloseSuccess = () => {
    setSuccess('');
  };
  useEffect(() => {
    const fetchTrucksAndDefinitions = async () => {
      try {
        const [trucksResponse, definitionsResponse] = await Promise.all([
          fetch('http://localhost:7006/api/trucks/'),
          fetch('http://localhost:7006/api/trucks/definitions'),
        ]);

        const trucksData = await trucksResponse.json();
        const definitionsData = await definitionsResponse.json();

        setTrucks(trucksData);
        setTruckDefinitions(definitionsData);
      } catch (error) {
        console.error('Error fetching trucks or definitions:', error);
      }
    };

    fetchTrucksAndDefinitions();
  }, []);

  const dataFiltered: TrucksResponse[] = applyFilter({
    inputData: trucks,
    comparator: getComparator(table.order, table.orderBy),
    filterName,
  });

  const notFound = !dataFiltered.length && !!filterName;

  const handleDelete = async (ids: string[]) => {
    try {
      const response = await fetch('http://localhost:7006/api/trucks/', {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(ids),
      });

      if (!response.ok) {
        throw new Error('Failed to delete trucks');
      }

      setTrucks(trucks.filter((truck) => !ids.includes(truck.id)));
      table.reset();
      setSuccess(ids.length === 1 ? 'Um caminhão removido.' : `${ids.length} caminhões removidos`);
    } catch (error) {
      console.error('Error deleting trucks:', error);
    } finally {
      setOpenDeleteDialog(false);
    }
  };

  const handleAdd100 = async () => {
    const response = await fetch(`http://localhost:7006/api/trucks/generate`);
    if (response.ok) {
      const data = await response.json();
      setSuccess(`${data.length} caminhões criados.`);
      setTrucks((prevTrucks) => [...data, ...prevTrucks]);
    }
  };
  const handleFormSubmit = async (newTruckData: any) => {
    try {
      console.log(newTruckData);
      if (currentTruckToEdit) {
        const response = await fetch(`http://localhost:7006/api/trucks/`, {
          method: 'PUT',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            id: currentTruckToEdit.id,
            model: newTruckData.model,
            manufacturingYear: newTruckData.manufacturingYear,
            chassisCode: newTruckData.chassisCode,
            color: newTruckData.color,
            plantIsoCode: newTruckData.plantName,
          }),
        });
        if (response.ok) {
          const updatedTruck = await response.json();
          setTrucks((prevTrucks) =>
            prevTrucks.map((truck) =>
              truck.id === currentTruckToEdit.id ? { ...truck, ...updatedTruck } : truck
            )
          );
          setSuccess('Caminhão atualizado.');
        } else {
          console.error('Failed to update truck');
        }
      } else {
        const response = await fetch('http://localhost:7006/api/trucks', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            model: newTruckData.model,
            manufacturingYear: newTruckData.manufacturingYear,
            chassisCode: newTruckData.chassisCode,
            color: newTruckData.color,
            plantIsoCode: newTruckData.plantName,
          }),
        });

        if (response.ok) {
          const createdTruck = await response.json();
          setSuccess('Caminhão criado.');
          setTrucks((prevTrucks) => [createdTruck, ...prevTrucks]);
        } else {
          console.error('Failed to create truck');
        }
      }
    } catch (error) {
      console.error('Error submitting truck form:', error);
    } finally {
      setOpenFormDialog(false);
      setCurrentTruckToEdit(null);
    }
  };

  const handleOpenNewDialog = () => {
    setCurrentTruckToEdit(null);
    setOpenFormDialog(true);
  };

  const handleOpenEditDialog = (truck: TrucksResponse) => {
    if (!truckDefinitions) {
      console.error('Truck definitions not loaded');
      return;
    }

    const modelDefinition = truckDefinitions.truckModels.find(
      (model) => model.description === truck.model
    );
    const plantDefinition = truckDefinitions.plantLocations.find(
      (plant) => plant.description === truck.plantName
    );

    if (!modelDefinition || !plantDefinition) {
      console.error('Model or Plant definition not found');
      return;
    }

    setCurrentTruckToEdit({
      id: truck.id,
      model: modelDefinition.value,
      manufacturingYear: truck.manufacturingYear,
      chassisCode: truck.chassisCode,
      color: truck.color,
      plantName: plantDefinition.value,
    });
    setOpenFormDialog(true);
  };

  return (
    <DashboardContent>
      <Box display="flex" alignItems="center" gap={2} mb={5}>
        <Typography variant="h4" flexGrow={1}>
          Caminhões
        </Typography>
        <Button variant="outlined" color="inherit" onClick={() => handleAdd100()}>
          Gerar 100 caminhões
        </Button>
        <Button
          variant="contained"
          color="inherit"
          startIcon={<Iconify icon="mingcute:add-line" />}
          onClick={() => handleOpenNewDialog()}
        >
          Novo Caminhão
        </Button>
      </Box>

      <Card>
        <UserTableToolbar
          numSelected={table.selected.length}
          filterName={filterName}
          onFilterName={(event: React.ChangeEvent<HTMLInputElement>) => {
            setFilterName(event.target.value);
            table.onResetPage();
          }}
          onDelete={() => setOpenDeleteDialog(true)}
        />

        <Scrollbar>
          <TableContainer sx={{ overflow: 'unset' }}>
            <Table sx={{ minWidth: 800 }}>
              <UserTableHead
                order={table.order}
                orderBy={table.orderBy}
                rowCount={trucks.length}
                numSelected={table.selected.length}
                onSort={table.onSort}
                onSelectAllRows={(checked) =>
                  table.onSelectAllRows(
                    checked,
                    trucks.map((truck) => truck.id)
                  )
                }
                headLabel={[
                  { id: 'model', label: 'Modelo' },
                  { id: 'manufacturingYear', label: 'Ano de fabricação' },
                  { id: 'chassisCode', label: 'Código do chassi' },
                  { id: 'color', label: 'Cor' },
                  { id: 'plantName', label: 'Planta' },
                  { id: '' },
                ]}
              />
              <TableBody>
                {dataFiltered
                  .slice(
                    table.page * table.rowsPerPage,
                    table.page * table.rowsPerPage + table.rowsPerPage
                  )
                  .map((row) => (
                    <TruckTableRow
                      onDelete={handleDelete}
                      truckDefinitions={truckDefinitions}
                      key={row.id}
                      row={row}
                      selected={table.selected.includes(row.id)}
                      onSelectRow={() => table.onSelectRow(row.id)}
                      onEdit={() => handleOpenEditDialog(row)}
                    />
                  ))}

                <TableEmptyRows
                  height={68}
                  emptyRows={emptyRows(table.page, table.rowsPerPage, trucks.length)}
                />

                {notFound && <TableNoData searchQuery={filterName} />}
              </TableBody>
            </Table>
          </TableContainer>
        </Scrollbar>

        <TablePagination
          component="div"
          page={table.page}
          count={trucks.length}
          rowsPerPage={table.rowsPerPage}
          onPageChange={table.onChangePage}
          rowsPerPageOptions={[5, 10, 25]}
          onRowsPerPageChange={table.onChangeRowsPerPage}
        />
      </Card>

      {/* Diálogo de Criação/Edição */}
      <TruckFormDialog
        open={openFormDialog}
        onClose={() => setOpenFormDialog(false)}
        onSubmit={handleFormSubmit}
        truckToEdit={currentTruckToEdit}
      />

      {/* Diálogo de Confirmação de Deleção */}
      <ConfirmDelete
        open={openDeleteDialog}
        onClose={() => setOpenDeleteDialog(false)}
        handleDelete={handleDelete}
        idsToDelete={table.selected}
      />
      <SimpleDialog text={success} onClose={handleCloseSuccess} />
    </DashboardContent>
  );
}

// ----------------------------------------------------------------------

export function useTable() {
  const [page, setPage] = useState(0);
  const [orderBy, setOrderBy] = useState('name');
  const [rowsPerPage, setRowsPerPage] = useState(5);
  const [selected, setSelected] = useState<string[]>([]);
  const [order, setOrder] = useState<'asc' | 'desc'>('asc');

  const onSort = useCallback(
    (id: string) => {
      const isAsc = orderBy === id && order === 'asc';
      setOrder(isAsc ? 'desc' : 'asc');
      setOrderBy(id);
    },
    [order, orderBy]
  );

  const reset = useCallback(() => {
    setSelected([]);
  }, []);
  const onSelectAllRows = useCallback((checked: boolean, newSelecteds: string[]) => {
    if (checked) {
      setSelected(newSelecteds);
      return;
    }
    setSelected([]);
  }, []);

  const onSelectRow = useCallback(
    (inputValue: string) => {
      const newSelected = selected.includes(inputValue)
        ? selected.filter((value) => value !== inputValue)
        : [...selected, inputValue];

      setSelected(newSelected);
    },
    [selected]
  );

  const onResetPage = useCallback(() => {
    setPage(0);
  }, []);

  const onChangePage = useCallback((event: unknown, newPage: number) => {
    setPage(newPage);
  }, []);

  const onChangeRowsPerPage = useCallback(
    (event: React.ChangeEvent<HTMLInputElement>) => {
      setRowsPerPage(parseInt(event.target.value, 10));
      onResetPage();
    },
    [onResetPage]
  );

  return {
    page,
    reset,
    order,
    onSort,
    orderBy,
    selected,
    rowsPerPage,
    onSelectRow,
    onResetPage,
    onChangePage,
    onSelectAllRows,
    onChangeRowsPerPage,
  };
}
