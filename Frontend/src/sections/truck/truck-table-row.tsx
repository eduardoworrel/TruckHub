import { useState, useCallback } from 'react';

import type { TruckDefinitions } from 'src/interfaces/truck';

import Box from '@mui/material/Box';
import Checkbox from '@mui/material/Checkbox';
import TableRow from '@mui/material/TableRow';
import TableCell from '@mui/material/TableCell';
import IconButton from '@mui/material/IconButton';
import Popover from '@mui/material/Popover';
import MenuList from '@mui/material/MenuList';
import MenuItem, { menuItemClasses } from '@mui/material/MenuItem';
import ReactCountryFlag from 'react-country-flag';

import { Iconify } from 'src/components/iconify';

import ConfirmDelete from './dialog/confirm-delete';

// ----------------------------------------------------------------------

export type TruckProps = {
  id: string;
  model: string;
  manufacturingYear: number;
  chassisCode: string;
  color: string;
  plantName: string;
  createdAt: string;
};

type UserTableRowProps = {
  row: TruckProps;
  selected: boolean;
  onSelectRow: () => void;
  onEdit: (truck: TruckProps) => void;
  onDelete: (ids: string[]) => void;
  truckDefinitions: TruckDefinitions | null;
};

export function TruckTableRow({
  row,
  selected,
  onSelectRow,
  onEdit,
  onDelete,
  truckDefinitions,
}: UserTableRowProps) {
  const [openPopover, setOpenPopover] = useState<HTMLButtonElement | null>(null);
  const [selectedIds, setSelectedIds] = useState<string[]>([]);
  const [openDeleteDialog, setOpenDeleteDialog] = useState(false);

  const handleOpenPopover = useCallback((event: React.MouseEvent<HTMLButtonElement>) => {
    setOpenPopover(event.currentTarget);
  }, []);

  const handleClosePopover = useCallback(() => {
    setOpenPopover(null);
  }, []);

  const handleDelete = async (ids: string[]) => {
    onDelete(ids);
  };

  const handleDeleteClick = () => {
    setSelectedIds([row.id]);
    setOpenDeleteDialog(true);
    handleClosePopover();
  };

  const handleEditClick = () => {
    onEdit(row); // Chamando a função onEdit com os dados do caminhão
    handleClosePopover();
  };

  const handleCloseDeleteDialog = () => {
    setOpenDeleteDialog(false);
  };

  return (
    <>
      <TableRow hover tabIndex={-1} role="checkbox" selected={selected}>
        <TableCell padding="checkbox">
          <Checkbox disableRipple checked={selected} onChange={onSelectRow} />
        </TableCell>

        <TableCell component="th" scope="row">
          <Box gap={2} display="flex" alignItems="center">
            {row.model}
          </Box>
        </TableCell>

        <TableCell>{row.manufacturingYear}</TableCell>
        <TableCell>{row.chassisCode}</TableCell>
        <TableCell>{row.color} </TableCell>

        <TableCell>
          <ReactCountryFlag
            countryCode={
              truckDefinitions?.plantLocations.filter((e) => e.description === row.plantName)[0]
                .name ?? ''
            }
          />{' '}
          {row.plantName}
        </TableCell>

        <TableCell align="right">
          <IconButton onClick={handleOpenPopover}>
            <Iconify icon="eva:more-vertical-fill" />
          </IconButton>
        </TableCell>
      </TableRow>

      <ConfirmDelete
        handleDelete={handleDelete}
        idsToDelete={selectedIds}
        open={openDeleteDialog}
        onClose={handleCloseDeleteDialog}
      />

      <Popover
        open={!!openPopover}
        anchorEl={openPopover}
        onClose={handleClosePopover}
        anchorOrigin={{ vertical: 'top', horizontal: 'left' }}
        transformOrigin={{ vertical: 'top', horizontal: 'right' }}
      >
        <MenuList
          disablePadding
          sx={{
            p: 0.5,
            gap: 0.5,
            width: 140,
            display: 'flex',
            flexDirection: 'column',
            [`& .${menuItemClasses.root}`]: {
              px: 1,
              gap: 2,
              borderRadius: 0.75,
              [`&.${menuItemClasses.selected}`]: { bgcolor: 'action.selected' },
            },
          }}
        >
          <MenuItem onClick={handleEditClick}>
            {' '}
            {/* Função de editar */}
            <Iconify icon="solar:pen-bold" />
            Edit
          </MenuItem>

          <MenuItem onClick={handleDeleteClick} sx={{ color: 'error.main' }}>
            <Iconify icon="solar:trash-bin-trash-bold" />
            Delete
          </MenuItem>
        </MenuList>
      </Popover>
    </>
  );
}
