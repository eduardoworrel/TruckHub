import * as React from 'react';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import Checkbox from '@mui/material/Checkbox';
import FormControlLabel from '@mui/material/FormControlLabel';

type ConfirmDeleteProps = {
  idsToDelete: string[];
  handleDelete: (ids: string[]) => void;
  open: boolean;
  onClose: () => void;
};

export default function ConfirmDelete({
  idsToDelete,
  handleDelete,
  open,
  onClose,
}: ConfirmDeleteProps) {
  const [checked, setChecked] = React.useState(false);

  const handleDeleteClick = () => {
    handleDelete(idsToDelete);
    onClose();
  };

  return (
    <Dialog
      open={open}
      onClose={onClose}
      aria-labelledby="alert-dialog-title"
      aria-describedby="alert-dialog-description"
    >
      <DialogTitle id="alert-dialog-title">Confirmação de Deleção</DialogTitle>
      <DialogContent>
        <DialogContentText id="alert-dialog-description">
          {idsToDelete.length > 1
            ? `Você está prestes a deletar ${idsToDelete.length} caminhões. Esta ação é irremediável.`
            : `Deletar um caminhão é uma ação irremediável.`}
          Tenha certeza de que deseja realizar esse procedimento antes de apertar em deletar
          permanentemente.
        </DialogContentText>
        <FormControlLabel
          control={<Checkbox checked={checked} onChange={(e) => setChecked(e.target.checked)} />}
          label="Eu entendo que esta ação não pode ser desfeita."
        />
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancelar</Button>
        <Button
          variant="contained"
          color="error"
          onClick={handleDeleteClick}
          disabled={!checked}
          autoFocus
        >
          Deletar permanentemente
        </Button>
      </DialogActions>
    </Dialog>
  );
}
